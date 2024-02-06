﻿/* ================================================================
   ---------------------------------------------------
   Project   :    Apex
   Publisher :    Renowned Games
   Developer :    Tamerlan Shakirov
   ---------------------------------------------------
   Copyright 2020-2023 Renowned Games All rights reserved.
   ================================================================ */

using System;
using UnityEngine;

namespace RenownedGames.Apex
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class ButtonWidthAttribute : ApexAttribute 
    {
        public readonly float width;

        public ButtonWidthAttribute(float width)
        {
            this.width = width;
            Alignment = TextAlignment.Center;
        }

        #region [Optional]
        /// <summary>
        /// Button alignment after resize width.
        /// </summary>
        public TextAlignment Alignment { get; set; }
        #endregion
    }
}