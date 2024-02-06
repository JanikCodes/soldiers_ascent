﻿/* ================================================================
   ---------------------------------------------------
   Project   :    Apex
   Publisher :    Renowned Games
   Developer :    Tamerlan Shakirov
   ---------------------------------------------------
   Copyright 2020-2023 Renowned Games All rights reserved.
   ================================================================ */

using System;

namespace RenownedGames.Apex
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class NotNullAttribute : DecoratorAttribute
    {
        public NotNullAttribute()
        {
            const string DEFAULT_MESSAGE = "{name} cannot be null!";
            Format = DEFAULT_MESSAGE;
            Height = 22;
            Style = MessageStyle.Error;
        }

        #region [Parameters]
        /// <summary>
        /// Custom message format. 
        /// Arguments: {name}
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// Height of help box message.
        /// </summary>
        public float Height { get; set; }

        public MessageStyle Style { get; set; }
        #endregion
    }
}