﻿/* ================================================================
   ---------------------------------------------------
   Project   :    Apex
   Publisher :    Renowned Games
   Developer :    Tamerlan Shakirov
   ---------------------------------------------------
   Copyright 2020-2023 Renowned Games All rights reserved.
   ================================================================ */

using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace RenownedGames.ApexEditor
{
    public class Button : MethodButton
    {
        private Action onClick;

        /// <summary>
        /// Implement this constructor to make initializations.
        /// </summary>
        /// <param name="serializedObject">Serialized object reference of this serialized member.</param>
        /// <param name="memberName">Member name of this serialized member.</param>
        public Button(SerializedObject serializedObject, string memberName) : base(serializedObject, memberName) 
        {
            MethodInfo methodInfo = GetMemberInfo() as MethodInfo;
            onClick = (Action)methodInfo.CreateDelegate(typeof(Action), GetDeclaringObject());
        }

        /// <summary>
        /// Called for rendering and handling button GUI.
        /// </summary>
        /// <param name="position">Rectangle position to draw button GUI.</param>
        protected override void OnButtonGUI(Rect position, GUIStyle style)
        {
            if (GUI.Button(position, GetLabel(), style))
            {
                Invoke();
            }
        }

        /// <summary>
        /// Invoke method.
        /// </summary>
        public void Invoke()
        {
            try
            {
                onClick.Invoke();
            }
            catch (Exception ex)
            {
                Debug.LogError($"Button click execution throw exception: {ex.Message}");
            }
        }
    }
}