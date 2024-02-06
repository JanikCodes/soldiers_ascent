﻿/* ================================================================
   ----------------------------------------------------------------
   Project   :   Apex
   Publisher :   Renowned Games
   Developer :   Tamerlan Shakirov
   ----------------------------------------------------------------
   Copyright 2020-2023 Renowned Games All rights reserved.
   ================================================================ */

namespace RenownedGames.Apex
{
    public sealed class OnObjectChangedAttribute : EditorMethodAttribute 
    {
        public OnObjectChangedAttribute()
        {
            DelayCall = false;
        }

        #region [Optional]
        /// <summary>
        /// Call callback after all inspectors update.
        /// </summary>
        public bool DelayCall { get; set; }
        #endregion
    }
}