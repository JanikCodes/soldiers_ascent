﻿/* ================================================================
  ---------------------------------------------------
  Project   :    Apex
  Publisher :    Renowned Games
  Developer :    Tamerlan Shakirov
  ---------------------------------------------------
  Copyright 2020-2023 Renowned Games All rights reserved.
  ================================================================ */

using RenownedGames.Apex;
using UnityEditor;

namespace RenownedGames.ApexEditor
{
    [ManipulatorTarget(typeof(LabelWidthAttribute))]
    public sealed class LabelWidthManipulator : MemberManipulator
    {
        private LabelWidthAttribute attribute;
        private float width;

        /// <summary>
        /// Called once when initializing member manipulator.
        /// </summary>
        /// <param name="member">Serialized member with ManipulatorAttribute.</param>
        /// <param name="ManipulatorAttribute">ManipulatorAttribute of serialized member.</param>
        public override void Initialize(SerializedMember member, ManipulatorAttribute ManipulatorAttribute)
        {
            attribute = ManipulatorAttribute as LabelWidthAttribute;
        }

        /// <summary>
        /// Called before rendering member GUI.
        /// </summary>
        public override void OnBeforeGUI()
        {
            width = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = attribute.width;
        }

        /// <summary>
        /// Called after rendering member GUI.
        /// </summary>
        public override void OnAfterGUI()
        {
            EditorGUIUtility.labelWidth = width;
        }
    }
}
