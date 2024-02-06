/* ================================================================
   ----------------------------------------------------------------
   Project   :   ExLib
   Company   :   Renowned Games
   Developer :   Tamerlan Shakirov
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

namespace RenownedGames.ExLibEditor
{
    public interface ISplitView
    {
        void BeginSplitView();

        void Split();

        void EndSplitView();
    }
}