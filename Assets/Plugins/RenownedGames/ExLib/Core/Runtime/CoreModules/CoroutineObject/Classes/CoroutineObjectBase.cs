﻿/* ================================================================
   ----------------------------------------------------------------
   Project   :   ExLib
   Company   :   Renowned Games
   Developer :   Tamerlan Shakirov
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using UnityEngine;

namespace RenownedGames.ExLib.Coroutines
{
    public abstract class CoroutineObjectBase : ICoroutineObjectBase
    {
        // Base coroutine object properties.
        protected MonoBehaviour owner;
        protected Coroutine coroutine;

        /// <summary>
        /// Constructor of CoroutineObjectBase.
        /// </summary>
        public CoroutineObjectBase(MonoBehaviour owner)
        {
            this.owner = owner;
        }

        /// <summary>
        /// Coroutine is processing.
        /// </summary>
        public bool IsProcessing()
        {
            return coroutine != null;
        }

        #region [Getter / Setter]
        public MonoBehaviour GetOwner()
        {
            return owner;
        }

        protected void SetOwner(MonoBehaviour value)
        {
            owner = value;
        }

        public Coroutine GetCoroutine()
        {
            return coroutine;
        }

        protected void SetCoroutine(Coroutine value)
        {
            coroutine = value;
        }
        #endregion
    }
}