/* ================================================================
   ----------------------------------------------------------------
   Project   :   ExLib
   Company   :   Renowned Games
   Developer :   Tamerlan Shakirov
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

namespace RenownedGames.ExLib.Coroutines
{
    public interface ICoroutineObjectBase
    {
        /// <summary>
        /// Coroutine is processing.
        /// </summary>
        bool IsProcessing();
    }
}