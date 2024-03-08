// Crest Ocean System

// Copyright 2022 Wave Harmonic Ltd

using System;

namespace Crest
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class ExecuteDuringEditModeAttribute : Attribute
    {
        [Flags]
        public enum Include
        {
            None,
            PrefabStage,
            BuildPipeline,
            All = PrefabStage | BuildPipeline,
        }

        public Include _including;

        public ExecuteDuringEditModeAttribute(Include including = Include.PrefabStage)
        {
            _including = including;
        }
    }
}
