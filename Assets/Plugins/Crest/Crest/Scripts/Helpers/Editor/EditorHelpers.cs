// Crest Ocean System

// Copyright 2021 Wave Harmonic Ltd

#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Crest.EditorHelpers
{
    /// <summary>
    /// Provides general helper functions for the editor.
    /// </summary>
    public static class EditorHelpers
    {
        static EditorWindow _lastGameOrSceneEditorWindow = null;

        /// <summary>
        /// Returns the scene view camera if the scene view is focused.
        /// </summary>
        public static Camera GetActiveSceneViewCamera()
        {
            Camera sceneCamera = null;

            if (EditorWindow.focusedWindow != null && (EditorWindow.focusedWindow.titleContent.text == "Scene" ||
                EditorWindow.focusedWindow.titleContent.text == "Game"))
            {
                _lastGameOrSceneEditorWindow = EditorWindow.focusedWindow;
            }

            // If scene view is focused, use its camera. This code is slightly ropey but seems to work ok enough.
            if (_lastGameOrSceneEditorWindow != null && _lastGameOrSceneEditorWindow.titleContent.text == "Scene")
            {
                var sceneView = SceneView.lastActiveSceneView;
                if (sceneView != null && !EditorApplication.isPlaying)
                {
                    sceneCamera = sceneView.camera;
                }
            }

            return sceneCamera;
        }

        public static LayerMask LayerMaskField(string label, LayerMask layerMask)
        {
            // Adapted from: http://answers.unity.com/answers/1387522/view.html
            var temporary = EditorGUILayout.MaskField(
                label,
                UnityEditorInternal.InternalEditorUtility.LayerMaskToConcatenatedLayersMask(layerMask),
                UnityEditorInternal.InternalEditorUtility.layers);
            return UnityEditorInternal.InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(temporary);
        }

        /// <summary>Attempts to get the scene view this camera is rendering.</summary>
        /// <returns>The scene view or null if not found.</returns>
        public static SceneView GetSceneViewFromSceneCamera(Camera camera)
        {
            foreach (SceneView sceneView in SceneView.sceneViews)
            {
                if (sceneView.camera == camera)
                {
                    return sceneView;
                }
            }

            return null;
        }

        /// <summary>Get time passed to animated materials.</summary>
        public static float GetShaderTime()
        {
            // When "Always Refresh" is disabled, Unity passes zero. Also uses realtimeSinceStartup:
            // https://github.com/Unity-Technologies/Graphics/blob/5743e39cdf0795cf7cbeb7ba8ffbbcc7ca200709/Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariablesGlobal.cs#L116
            return !Application.isPlaying && SceneView.lastActiveSceneView != null &&
                !SceneView.lastActiveSceneView.sceneViewState.alwaysRefresh ? 0f : Time.realtimeSinceStartup;
        }

        public static GameObject GetGameObject(SerializedObject serializedObject)
        {
            // We will either get the component or the GameObject it is attached to.
            return serializedObject.targetObject is GameObject
                ? serializedObject.targetObject as GameObject
                : (serializedObject.targetObject as Component).gameObject;
        }
    }
}

#endif
