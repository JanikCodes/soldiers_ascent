﻿/* ================================================================
   ----------------------------------------------------------------
   Project   :   Apex
   Publisher :   Renowned Games
   Developer :   Tamerlan Shakirov
   ----------------------------------------------------------------
   Copyright 2020-2023 Renowned Games All rights reserved.
   ================================================================ */

using RenownedGames.Apex;

namespace RenownedGames.ApexEditor
{
    public interface IManipulatorInitializaton
    {
        /// <summary>
        /// Called once when initializing member manipulator.
        /// </summary>
        /// <param name="member">Serialized member with ManipulatorAttribute.</param>
        /// <param name="ManipulatorAttribute">ManipulatorAttribute of serialized member.</param>
        void Initialize(SerializedMember member, ManipulatorAttribute ManipulatorAttribute);
    }
}