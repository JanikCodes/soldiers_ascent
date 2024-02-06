﻿/* ================================================================
   ----------------------------------------------------------------
   Project   :   Apex
   Publisher :   Renowned Games
   Developer :   Tamerlan Shakirov
   ----------------------------------------------------------------
   Copyright 2020-2023 Renowned Games All rights reserved.
   ================================================================ */

using RenownedGames.Apex;
using UnityEditor;

namespace RenownedGames.ApexEditor
{
    [ManipulatorTarget(typeof(DisableInEditorModeAttribute))]
    public sealed class DisableInEditorModeManipulator : MemberManipulator
    {
        /// <summary>
        /// Called before rendering member GUI.
        /// </summary>
        public override void OnBeforeGUI()
        {
            EditorGUI.BeginDisabledGroup(!EditorApplication.isPlaying || !EditorApplication.isPaused);
        }

        /// <summary>
        /// Called after rendering member GUI.
        /// </summary>
        public override void OnAfterGUI()
        {
            EditorGUI.EndDisabledGroup();
        }
    }
}