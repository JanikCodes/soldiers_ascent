// Crest Ocean System

// Copyright 2021 Wave Harmonic Ltd

#if CREST_HDRP

namespace Crest
{
    using UnityEngine;
    using UnityEngine.Rendering.HighDefinition;
    using UnityEngine.Rendering;
    using UnityEngine.Experimental.Rendering;

    internal class UnderwaterMaskPassHDRP : CustomPass
    {
        const string k_Name = "Underwater Mask";
        const string k_ShaderPath = "Hidden/Crest/Underwater/Ocean Mask HDRP";
        internal const string k_ShaderPathWaterVolumeGeometry = "Hidden/Crest/Water Volume Geometry HDRP";

        Material _oceanMaskMaterial;
        RTHandle _maskTexture;
        RTHandle _depthTexture;
        RTHandle _volumeFrontFaceRT;
        RTHandle _volumeBackFaceRT;
        RenderTargetIdentifier _maskTarget;
        RenderTargetIdentifier _depthTarget;
        internal RenderTargetIdentifier _volumeFrontFaceTarget;
        internal RenderTargetIdentifier _volumeBackFaceTarget;
        Plane[] _cameraFrustumPlanes;

        static GameObject s_GameObject;
        UnderwaterRenderer _renderer;

        public static void Enable()
        {
            CustomPassHelpers.CreateOrUpdate<UnderwaterMaskPassHDRP>(ref s_GameObject, k_Name, CustomPassInjectionPoint.BeforeRendering);
            RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;
            RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
        }

        public static void Disable()
        {
            // It should be safe to rely on this reference for this reference to fail.
            if (s_GameObject != null)
            {
                s_GameObject.SetActive(false);
            }

            UnderwaterRenderer.DisableOceanMaskKeywords();
            RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;
        }

        internal static void SetUp(UnderwaterRenderer renderer)
        {
            renderer._volumeMaterial = CoreUtils.CreateEngineMaterial(k_ShaderPathWaterVolumeGeometry);
            renderer.SetUpFixMaskArtefactsShader();
        }

        internal static void CleanUp(UnderwaterRenderer renderer)
        {
            if (renderer._volumeMaterial != null)
            {
                CoreUtils.Destroy(renderer._volumeMaterial);
            }
        }

        protected override void Setup(ScriptableRenderContext renderContext, CommandBuffer cmd)
        {
            _oceanMaskMaterial = CoreUtils.CreateEngineMaterial(k_ShaderPath);

            _maskTexture = RTHandles.Alloc
            (
                scaleFactor: Vector2.one,
                slices: TextureXR.slices,
                dimension: TextureXR.dimension,
                colorFormat: GraphicsFormat.R16_SFloat,
                enableRandomWrite: true,
                useDynamicScale: true,
                name: "Crest Ocean Mask"
            );

            _maskTarget = new RenderTargetIdentifier
            (
                _maskTexture,
                mipLevel: 0,
                CubemapFace.Unknown,
                depthSlice: -1 // Bind all XR slices.
            );

            _depthTexture = RTHandles.Alloc
            (
                scaleFactor: Vector2.one,
                slices: TextureXR.slices,
                dimension: TextureXR.dimension,
                depthBufferBits: DepthBits.Depth24,
                colorFormat: GraphicsFormat.R8_UNorm, // This appears to be used for depth.
                enableRandomWrite: false,
                useDynamicScale: true,
                name: "Crest Ocean Mask Depth"
            );

            _depthTarget = new RenderTargetIdentifier
            (
                _depthTexture,
                mipLevel: 0,
                CubemapFace.Unknown,
                depthSlice: -1 // Bind all XR slices.
            );

            SetUpVolumeTextures();
        }

        protected override void Cleanup()
        {
            CoreUtils.Destroy(_oceanMaskMaterial);
            _maskTexture.Release();
            _depthTexture.Release();

            CleanUpVolumeTextures();
        }

        void SetUpVolumeTextures()
        {
            if (_volumeFrontFaceRT == null)
            {
                _volumeFrontFaceRT = RTHandles.Alloc
                (
                    scaleFactor: Vector2.one,
                    slices: TextureXR.slices,
                    dimension: TextureXR.dimension,
                    depthBufferBits: DepthBits.Depth24,
                    colorFormat: GraphicsFormat.R8_UNorm, // This appears to be used for depth.
                    enableRandomWrite: false,
                    useDynamicScale: true,
                    name: "_CrestVolumeFrontFaceTexture"
                );

                _volumeFrontFaceTarget = new RenderTargetIdentifier
                (
                    _volumeFrontFaceRT,
                    mipLevel: 0,
                    CubemapFace.Unknown,
                    depthSlice: -1 // Bind all XR slices.
                );
            }

            if (_volumeBackFaceRT == null)
            {
                _volumeBackFaceRT = RTHandles.Alloc
                (
                    scaleFactor: Vector2.one,
                    slices: TextureXR.slices,
                    dimension: TextureXR.dimension,
                    depthBufferBits: DepthBits.Depth24,
                    colorFormat: GraphicsFormat.R8_UNorm, // This appears to be used for depth.
                    enableRandomWrite: false,
                    useDynamicScale: true,
                    name: "_CrestVolumeBackFaceTexture"
                );

                _volumeBackFaceTarget = new RenderTargetIdentifier
                (
                    _volumeBackFaceRT,
                    mipLevel: 0,
                    CubemapFace.Unknown,
                    depthSlice: -1 // Bind all XR slices.
                );
            }
        }

        void CleanUpVolumeTextures()
        {
            _volumeFrontFaceRT?.Release();
            _volumeFrontFaceRT = null;
            _volumeBackFaceRT?.Release();
            _volumeBackFaceRT = null;
        }

        static void OnBeginCameraRendering(ScriptableRenderContext context, Camera camera)
        {
            var renderer = UnderwaterRenderer.Get(camera);

            if (renderer == null)
            {
                UnderwaterRenderer.DisableOceanMaskKeywords();
                return;
            }

            Helpers.SetGlobalKeyword(UnderwaterRenderer.k_KeywordVolume2D, renderer._mode == UnderwaterRenderer.Mode.Portal);
            Helpers.SetGlobalKeyword(UnderwaterRenderer.k_KeywordVolumeHasBackFace, renderer._mode == UnderwaterRenderer.Mode.Volume
                || renderer._mode == UnderwaterRenderer.Mode.VolumeFlyThrough);
        }

        protected override void Execute(CustomPassContext context)
        {
            var camera = context.hdCamera.camera;
            _renderer = UnderwaterRenderer.Get(camera);

            if (UnderwaterPostProcessHDRP.Instance == null)
            {
                if (!_renderer || !_renderer.IsActive)
                {
                    return;
                }
            }
            else
            {
                // HDRP PP compatibility.
                if (!ReferenceEquals(camera, OceanRenderer.Instance.ViewCamera) || camera.cameraType != CameraType.Game)
                {
                    return;
                }
            }

            var commandBuffer = context.cmd;

            if (!Helpers.MaskIncludesLayer(camera.cullingMask, OceanRenderer.Instance.Layer))
            {
                return;
            }

            if (_cameraFrustumPlanes == null)
            {
                _cameraFrustumPlanes = GeometryUtility.CalculateFrustumPlanes(camera);
            }

            // This property is either on the UnderwaterRenderer or UnderwaterPostProcessHDRP.
            var debugDisableOceanMask = false;
            var farPlaneMultiplier = 1.0f;
            var enableShaderAPI = false;

            if (_renderer != null)
            {
                debugDisableOceanMask = _renderer._debug._disableOceanMask;
                farPlaneMultiplier = _renderer._farPlaneMultiplier;
                enableShaderAPI = _renderer.EnableShaderAPI;

                _renderer.SetUpVolume(_oceanMaskMaterial);

                // Populate water volume before mask so we can use the stencil.
                if (_renderer._mode != UnderwaterRenderer.Mode.FullScreen && _renderer._volumeGeometry != null)
                {
                    SetUpVolumeTextures();
                    _renderer.PopulateVolume(commandBuffer, _volumeFrontFaceTarget, _volumeBackFaceTarget, null, _volumeFrontFaceRT.rtHandleProperties.currentViewportSize);
                    // Copy only the stencil by copying everything and clearing depth.
                    commandBuffer.CopyTexture(_renderer._mode == UnderwaterRenderer.Mode.Portal ? _volumeFrontFaceTarget : _volumeBackFaceTarget, _depthTarget);
                    Helpers.Blit(commandBuffer, _depthTarget, Helpers.UtilityMaterial, (int)Helpers.UtilityPass.ClearDepth);
                }

                _renderer.SetUpMask(commandBuffer, _maskTarget, _depthTarget);
                // For dynamic scaling to work.
                CoreUtils.SetViewport(commandBuffer, _maskTexture);
            }
            else
            {
                debugDisableOceanMask = UnderwaterPostProcessHDRP.Instance._disableOceanMask.value;
                farPlaneMultiplier = UnderwaterPostProcessHDRP.Instance._farPlaneMultiplier.value;

                CoreUtils.SetRenderTarget(commandBuffer, _maskTexture, _depthTexture);
                CoreUtils.ClearRenderTarget(commandBuffer, ClearFlag.All, Color.black);
                commandBuffer.SetGlobalTexture(UnderwaterRenderer.ShaderIDs.s_CrestOceanMaskTexture, _maskTexture);
                commandBuffer.SetGlobalTexture(UnderwaterRenderer.ShaderIDs.s_CrestOceanMaskDepthTexture, _depthTexture);
            }

            UnderwaterRenderer.PopulateOceanMask(
                commandBuffer,
                camera,
                OceanRenderer.Instance.Tiles,
                _cameraFrustumPlanes,
                _oceanMaskMaterial,
                farPlaneMultiplier,
                enableShaderAPI,
                debugDisableOceanMask
            );

            if (_renderer != null)
            {
                var size = _maskTexture.GetScaledSize(_maskTexture.rtHandleProperties.currentViewportSize);
                var descriptor = new RenderTextureDescriptor(size.x, size.y);
                _renderer.FixMaskArtefacts(commandBuffer, descriptor, _maskTarget);
            }
        }
    }
}

#endif // CREST_HDRP
