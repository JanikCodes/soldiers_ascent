﻿/* ================================================================
   ----------------------------------------------------------------
   Project   :   Apex
   Publisher :   Renowned Games
   Developer :   Tamerlan Shakirov
   ----------------------------------------------------------------
   Copyright 2020-2023 Renowned Games All rights reserved.
   ================================================================ */

using UnityEditor;
using UnityEngine;
using RenownedGames.Apex;

namespace RenownedGames.ApexEditor
{
    [ViewTarget(typeof(ToggleLeftAttribute))]
    public sealed class ToggleLeftView : FieldView
    {
        /// <summary>
        /// Called for drawing element view GUI.
        /// </summary>
        /// <param name="position">Position of the Serialized field.</param>
        /// <param name="serializedField">Serialized field with ViewAttribute.</param>
        /// <param name="label">Label of Serialized field.</param>
        public override void OnGUI(Rect position, SerializedField serializedField, GUIContent label)
        {
            serializedField.GetSerializedProperty().boolValue = EditorGUI.ToggleLeft(position, label, serializedField.GetSerializedProperty().boolValue);
        }
    }
}