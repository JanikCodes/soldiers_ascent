// Crest Ocean System

// Copyright 2021 Wave Harmonic Ltd

#if CREST_URP

namespace Crest
{
    using Crest.Internal;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Rendering;
    using UnityEngine.Rendering.Universal;

    internal class UnderwaterEffectPassURP : ScriptableRenderPass
    {
        const string SHADER_UNDERWATER_EFFECT = "Hidden/Crest/Underwater/Underwater Effect URP";
        static readonly int sp_TemporaryColor = Shader.PropertyToID("_TemporaryColor");

        readonly PropertyWrapperMaterial _underwaterEffectMaterial;
        RenderTargetIdentifier _colorTarget;
        RenderTargetIdentifier _depthTarget;
        RenderTargetIdentifier _depthStencilTarget = new RenderTargetIdentifier(UnderwaterRenderer.ShaderIDs.s_CrestWaterVolumeStencil, 0, CubemapFace.Unknown, -1);
        RenderTargetIdentifier _temporaryColorTarget = new RenderTargetIdentifier(sp_TemporaryColor, 0, CubemapFace.Unknown, -1);
        RenderTargetIdentifier _cameraDepthTarget = new RenderTargetIdentifier(BuiltinRenderTextureType.CameraTarget);
        RenderTexture _depthStencil;
        RenderTexture _temporaryColor;

        bool _firstRender = true;
        Camera _camera;

        static int s_InstanceCount;
        static RenderObjectsWithoutFogPass s_ApplyFogToTransparentObjects;
        UnderwaterRenderer _underwaterRenderer;

        public UnderwaterEffectPassURP()
        {
            renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
            _underwaterEffectMaterial = new PropertyWrapperMaterial(SHADER_UNDERWATER_EFFECT);
            _underwaterEffectMaterial.material.hideFlags = HideFlags.HideAndDontSave;
        }

        internal void CleanUp()
        {
            CoreUtils.Destroy(_underwaterEffectMaterial.material);
        }

        public void Enable(UnderwaterRenderer underwaterRenderer)
        {
            s_InstanceCount++;
            _underwaterRenderer = underwaterRenderer;

            if (s_ApplyFogToTransparentObjects == null)
            {
                s_ApplyFogToTransparentObjects = new RenderObjectsWithoutFogPass();
            }

            RenderPipelineManager.beginCameraRendering -= EnqueuePass;
            RenderPipelineManager.beginCameraRendering += EnqueuePass;
        }

        public void Disable()
        {
            if (--s_InstanceCount <= 0)
            {
                RenderPipelineManager.beginCameraRendering -= EnqueuePass;
            }
        }

        static void EnqueuePass(ScriptableRenderContext context, Camera camera)
        {
            var ur = UnderwaterRenderer.Get(camera);

            if (!ur || !ur.IsActive)
            {
                return;
            }

            if (!Helpers.MaskIncludesLayer(camera.cullingMask, OceanRenderer.Instance.Layer))
            {
                return;
            }

            // Enqueue the pass. This happens every frame.
            var renderer = camera.GetUniversalAdditionalCameraData().scriptableRenderer;
            renderer.EnqueuePass(ur._urpEffectPass);
            if (ur.EnableShaderAPI)
            {
                renderer.EnqueuePass(s_ApplyFogToTransparentObjects);
                s_ApplyFogToTransparentObjects._underwaterRenderer = ur;
            }
        }

        // Called before Configure.
        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
#if UNITY_2023_1_OR_NEWER
            _colorTarget = renderingData.cameraData.renderer.cameraColorTargetHandle;
            _depthTarget = renderingData.cameraData.renderer.cameraDepthTargetHandle;
#else
#pragma warning disable 618
            _colorTarget = renderingData.cameraData.renderer.cameraColorTarget;
            _depthTarget = renderingData.cameraData.renderer.cameraDepthTarget;
#pragma warning restore 618
#endif
            _camera = renderingData.cameraData.camera;
        }

        // Called before Execute.
        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraDescriptor)
        {
            // Calling ConfigureTarget is recommended by Unity, but that means it can only use it once? Also Blit breaks
            // XR SPI. Using SetRenderTarget and custom Blit instead.
            {
                var descriptor = cameraDescriptor;
                descriptor.msaaSamples = 1;
                _temporaryColor = RenderTexture.GetTemporary(descriptor);
                _temporaryColorTarget = new RenderTargetIdentifier(_temporaryColor, 0, CubemapFace.Unknown, -1);
            }

            if (_underwaterRenderer.UseStencilBufferOnEffect)
            {
                var descriptor = cameraDescriptor;
                descriptor.colorFormat = RenderTextureFormat.Depth;
                descriptor.depthBufferBits = 24;
                descriptor.SetMSAASamples(_camera);
                descriptor.bindMS = descriptor.msaaSamples > 1;

                _depthStencil = RenderTexture.GetTemporary(descriptor);
                _depthStencilTarget = new RenderTargetIdentifier(_depthStencil, 0, CubemapFace.Unknown, -1);
            }
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            var camera = renderingData.cameraData.camera;

            // Ensure legacy underwater fog is disabled.
            if (_firstRender)
            {
                OceanRenderer.Instance.OceanMaterial.DisableKeyword("_OLD_UNDERWATER");
            }

#if UNITY_EDITOR
            if (!UnderwaterRenderer.IsFogEnabledForEditorCamera(camera))
            {
                return;
            }
#endif

            CommandBuffer commandBuffer = CommandBufferPool.Get("Underwater Effect");

            UnderwaterRenderer.UpdatePostProcessMaterial(
                _underwaterRenderer,
                _underwaterRenderer._mode,
                camera,
                _underwaterEffectMaterial,
                _underwaterRenderer._sphericalHarmonicsData,
                _underwaterRenderer._meniscus,
                _firstRender || _underwaterRenderer._copyOceanMaterialParamsEachFrame,
                _underwaterRenderer._debug._viewOceanMask,
                _underwaterRenderer._debug._viewStencil,
                _underwaterRenderer._filterOceanData,
                ref _underwaterRenderer._currentOceanMaterial,
                _underwaterRenderer.EnableShaderAPI
            );

            // Create a separate stencil buffer context by copying the depth texture.
            if (_underwaterRenderer.UseStencilBufferOnEffect)
            {
                commandBuffer.SetRenderTarget(_depthStencilTarget);
                Helpers.Blit(commandBuffer, _depthStencilTarget, Helpers.UtilityMaterial, (int)Helpers.UtilityPass.CopyDepth);
            }

            // Copy color buffer.
            if (Helpers.IsMSAAEnabled(camera))
            {
                Helpers.Blit(commandBuffer, _temporaryColorTarget, Helpers.UtilityMaterial, (int)Helpers.UtilityPass.CopyColor);
            }
            else
            {
                commandBuffer.CopyTexture(_colorTarget, _temporaryColorTarget);
            }

            commandBuffer.SetGlobalTexture(UnderwaterRenderer.ShaderIDs.s_CrestCameraColorTexture, _temporaryColorTarget);

            if (_underwaterRenderer.UseStencilBufferOnEffect)
            {
                commandBuffer.SetRenderTarget(_colorTarget, _depthStencilTarget);
            }
            else
            {
#if UNITY_2022_1_OR_NEWER
                commandBuffer.SetRenderTarget(_colorTarget, _depthTarget);
#elif UNITY_2021_3_OR_NEWER
                // Some modes require a depth buffer but getting the depth buffer to bind depends on whether depth
                // prepass is used in addition to one of Unity's render passes being active like SSAO. Turns out that if
                // the depth target type is camera target, then we must rely on implicit binding below.
                if (_underwaterRenderer._mode != UnderwaterRenderer.Mode.FullScreen && _cameraDepthTarget != _depthTarget)
                {
                    commandBuffer.SetRenderTarget(_colorTarget, _depthTarget);
                }
                else
                {
                    commandBuffer.SetRenderTarget(_colorTarget);
                }
#else
                if (_underwaterRenderer._mode != UnderwaterRenderer.Mode.FullScreen && renderingData.cameraData.xrRendering)
                {
                    commandBuffer.SetRenderTarget(_colorTarget, _depthTarget);
                }
                else
                {
                    commandBuffer.SetRenderTarget(_colorTarget);
                }
#endif
            }

            _underwaterRenderer.ExecuteEffect(commandBuffer, _underwaterEffectMaterial.material);

            context.ExecuteCommandBuffer(commandBuffer);

            RenderTexture.ReleaseTemporary(_temporaryColor);
            RenderTexture.ReleaseTemporary(_depthStencil);

            if (_underwaterRenderer.UseStencilBufferOnEffect)
            {
                commandBuffer.ReleaseTemporaryRT(UnderwaterRenderer.ShaderIDs.s_CrestWaterVolumeStencil);
            }

            CommandBufferPool.Release(commandBuffer);

            _firstRender = false;
        }

        class RenderObjectsWithoutFogPass : ScriptableRenderPass
        {
            FilteringSettings m_FilteringSettings;
            internal UnderwaterRenderer _underwaterRenderer;

            static readonly List<ShaderTagId> m_ShaderTagIdList = new List<ShaderTagId>()
            {
                new ShaderTagId("SRPDefaultUnlit"),
                new ShaderTagId("UniversalForward"),
                new ShaderTagId("UniversalForwardOnly"),
                new ShaderTagId("LightweightForward"),
            };

            public RenderObjectsWithoutFogPass()
            {
                renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
                m_FilteringSettings = new FilteringSettings(RenderQueueRange.transparent, 0);
            }

            public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
            {
                var drawingSettings = CreateDrawingSettings
                (
                    m_ShaderTagIdList,
                    ref renderingData,
                    SortingCriteria.CommonTransparent
                );

                m_FilteringSettings.layerMask = _underwaterRenderer._transparentObjectLayers;

                var buffer = CommandBufferPool.Get();

                // Disable Unity's fog keywords as there is no option to ignore fog for the Shader Graph.
                if (RenderSettings.fog)
                {
                    switch (RenderSettings.fogMode)
                    {
                        case FogMode.Exponential:
                            buffer.DisableShaderKeyword("FOG_EXP");
                            break;
                        case FogMode.Linear:
                            buffer.DisableShaderKeyword("FOG_LINEAR");
                            break;
                        case FogMode.ExponentialSquared:
                            buffer.DisableShaderKeyword("FOG_EXP2");
                            break;
                    }
                }

                buffer.EnableShaderKeyword("CREST_UNDERWATER_OBJECTS_PASS");
                // If we want anything to apply to DrawRenderers, it has to be executed before:
                // https://docs.unity3d.com/ScriptReference/Rendering.ScriptableRenderContext.DrawRenderers.html
                context.ExecuteCommandBuffer(buffer);
                buffer.Clear();

                context.DrawRenderers(renderingData.cullResults, ref drawingSettings, ref m_FilteringSettings);

                // Revert fog keywords.
                if (RenderSettings.fog)
                {
                    switch (RenderSettings.fogMode)
                    {
                        case FogMode.Exponential:
                            buffer.EnableShaderKeyword("FOG_EXP");
                            break;
                        case FogMode.Linear:
                            buffer.EnableShaderKeyword("FOG_LINEAR");
                            break;
                        case FogMode.ExponentialSquared:
                            buffer.EnableShaderKeyword("FOG_EXP2");
                            break;
                    }
                }

                buffer.DisableShaderKeyword("CREST_UNDERWATER_OBJECTS_PASS");
                context.ExecuteCommandBuffer(buffer);

                CommandBufferPool.Release(buffer);
            }
        }
    }
}

#endif // CREST_URP
