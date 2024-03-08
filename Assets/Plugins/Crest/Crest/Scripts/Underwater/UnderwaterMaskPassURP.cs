// Crest Ocean System

// Copyright 2021 Wave Harmonic Ltd

#if CREST_URP

namespace Crest
{
    using UnityEngine;
    using UnityEngine.Rendering;
    using UnityEngine.Rendering.Universal;

    internal class UnderwaterMaskPassURP : ScriptableRenderPass
    {
        const string k_ShaderPathOceanMask = "Hidden/Crest/Underwater/Ocean Mask URP";

        readonly PropertyWrapperMaterial _oceanMaskMaterial;

        static int s_InstanceCount;
        UnderwaterRenderer _underwaterRenderer;

        public UnderwaterMaskPassURP()
        {
            // Will always execute and matrices will be ready.
#if UNITY_2021_3_OR_NEWER
            renderPassEvent = RenderPassEvent.BeforeRenderingPrePasses;
#else
            renderPassEvent = RenderPassEvent.BeforeRenderingPrepasses;
#endif
            _oceanMaskMaterial = new PropertyWrapperMaterial(k_ShaderPathOceanMask);
            _oceanMaskMaterial.material.hideFlags = HideFlags.HideAndDontSave;
        }

        internal void CleanUp()
        {
            CoreUtils.Destroy(_oceanMaskMaterial.material);
        }

        public void Enable(UnderwaterRenderer underwaterRenderer)
        {
            s_InstanceCount++;
            _underwaterRenderer = underwaterRenderer;
            _underwaterRenderer.OnEnableMask();

            RenderPipelineManager.beginCameraRendering -= EnqueuePass;
            RenderPipelineManager.beginCameraRendering += EnqueuePass;
        }

        public void Disable()
        {
            _underwaterRenderer.OnDisableMask();

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
            camera.GetUniversalAdditionalCameraData().scriptableRenderer.EnqueuePass(ur._urpMaskPass);
        }

        // Called before Configure.
        public override void OnCameraSetup(CommandBuffer buffer, ref RenderingData renderingData)
        {
            var descriptor = renderingData.cameraData.cameraTargetDescriptor;
            // Keywords and other things.
            _underwaterRenderer.SetUpVolume(_oceanMaskMaterial.material);
            _underwaterRenderer.SetUpMaskTextures(descriptor);
            if (_underwaterRenderer._mode != UnderwaterRenderer.Mode.FullScreen && _underwaterRenderer._volumeGeometry != null)
            {
                _underwaterRenderer.SetUpVolumeTextures(descriptor);
            }
        }

        // Called before Execute.
        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
#if !UNITY_2023_1_OR_NEWER
#pragma warning disable 618
            ConfigureTarget(_underwaterRenderer._maskTarget, _underwaterRenderer._depthTarget);
#pragma warning restore 618
#endif
            ConfigureClear(ClearFlag.All, Color.black);
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            var camera = renderingData.cameraData.camera;

            XRHelpers.Update(camera);
            XRHelpers.UpdatePassIndex(ref UnderwaterRenderer.s_xrPassIndex);

            CommandBuffer commandBuffer = CommandBufferPool.Get("Ocean Mask");


            // Populate water volume before mask so we can use the stencil.
            if (_underwaterRenderer._mode != UnderwaterRenderer.Mode.FullScreen && _underwaterRenderer._volumeGeometry != null)
            {
                _underwaterRenderer.PopulateVolume(commandBuffer, _underwaterRenderer._volumeFrontFaceTarget, _underwaterRenderer._volumeBackFaceTarget);
                // Copy only the stencil by copying everything and clearing depth.
                commandBuffer.CopyTexture(_underwaterRenderer._mode == UnderwaterRenderer.Mode.Portal ? _underwaterRenderer._volumeFrontFaceTarget : _underwaterRenderer._volumeBackFaceTarget, _underwaterRenderer._depthTarget);
                Helpers.Blit(commandBuffer, _underwaterRenderer._depthTarget, Helpers.UtilityMaterial, (int)Helpers.UtilityPass.ClearDepth);
            }

            _underwaterRenderer.SetUpMask(commandBuffer, _underwaterRenderer._maskTarget, _underwaterRenderer._depthTarget);
            UnderwaterRenderer.PopulateOceanMask(
                commandBuffer,
                camera,
                OceanRenderer.Instance.Tiles,
                _underwaterRenderer._cameraFrustumPlanes,
                _oceanMaskMaterial.material,
                _underwaterRenderer._farPlaneMultiplier,
                _underwaterRenderer.EnableShaderAPI,
                _underwaterRenderer._debug._disableOceanMask
            );

            _underwaterRenderer.FixMaskArtefacts
            (
                commandBuffer,
                renderingData.cameraData.cameraTargetDescriptor,
                _underwaterRenderer._maskTarget
            );

            context.ExecuteCommandBuffer(commandBuffer);
            CommandBufferPool.Release(commandBuffer);
        }
    }
}

#endif // CREST_URP
