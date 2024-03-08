// Crest Ocean System

// Copyright 2021 Wave Harmonic Ltd

#if CREST_HDRP

namespace Crest
{
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.Rendering.HighDefinition;

    class SampleShadowsHDRP : CustomPass
    {
        static GameObject gameObject;
        static readonly string Name = "Sample Shadows";

        // These values come from unity_MatrxVP value in the frame debugger. unity_MatrxVP is marked as legacy and
        // breaks XR SPI. It is defined in:
        // "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/EditorShaderVariables.hlsl"
        static readonly Matrix4x4 s_Matrix = new Matrix4x4
        (
            new Vector4(2f, 0f, 0f, 0f),
            new Vector4(0f, -2f, 0f, 0f),
            new Vector4(0f, 0f, 0.00990099f, 0f),
            new Vector4(-1f, 1f, 0.990099f, 1f)
        );

        static readonly int sp_CrestViewProjectionMatrix = Shader.PropertyToID("_CrestViewProjectionMatrix");

        int _xrTargetEyeIndex = -1;

        protected override void Execute(CustomPassContext context)
        {
            var ocean = OceanRenderer.Instance;

            if (ocean == null || ocean._lodDataShadow == null)
            {
                return;
            }

#if UNITY_EDITOR
            if (!OceanRenderer.IsWithinEditorUpdate || EditorApplication.isPaused)
            {
                return;
            }
#endif

            var camera = context.hdCamera.camera;

            // Custom passes execute for every camera. We only support one camera for now.
            if (!ReferenceEquals(camera, ocean.ViewCamera)) return;
            // TODO: bail when not executing for main light or when no main light exists?
            // if (renderingData.lightData.mainLightIndex == -1) return;

            camera.TryGetComponent<HDAdditionalCameraData>(out var cameraData);

            if (cameraData != null && cameraData.xrRendering)
            {
                XRHelpers.UpdatePassIndex(ref _xrTargetEyeIndex);

                // Skip the right eye as data is not stereo.
                if (_xrTargetEyeIndex == 1)
                {
                    return;
                }
            }

            // Disable for XR SPI otherwise input will not have correct world position.
            if (cameraData != null && cameraData.xrRendering && XRHelpers.IsSinglePass)
            {
                context.cmd.DisableShaderKeyword("STEREO_INSTANCING_ON");
            }

            // We cannot seem to override this matrix so a reference manually.
            context.cmd.SetGlobalMatrix(sp_CrestViewProjectionMatrix, s_Matrix);
            ocean._lodDataShadow.BuildCommandBuffer(ocean, context.cmd);

            // Restore matrices otherwise remaining render will have incorrect matrices. Each pass is responsible for
            // restoring matrices if required.
            context.cmd.SetViewProjectionMatrices(camera.worldToCameraMatrix, camera.projectionMatrix);

            // Restore XR SPI as we cannot rely on remaining pipeline to do it for us.
            if (cameraData != null && cameraData.xrRendering && XRHelpers.IsSinglePass)
            {
                context.cmd.EnableShaderKeyword("STEREO_INSTANCING_ON");
            }
        }

        public static void Enable()
        {
            CustomPassHelpers.CreateOrUpdate<SampleShadowsHDRP>(ref gameObject, Name, CustomPassInjectionPoint.BeforeTransparent);
        }

        public static void Disable()
        {
            // It should be safe to rely on this reference for this reference to fail.
            if (gameObject != null)
            {
                gameObject.SetActive(false);
            }
        }
    }
}

#endif // CREST_HDRP
