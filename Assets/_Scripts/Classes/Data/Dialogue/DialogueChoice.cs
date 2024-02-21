using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that is used to generate the ScriptableObject <see cref="DialogueChoiceSO"/> at runtime from.
/// </summary>
[Serializable]
public class DialogueChoiceData : BaseData
{
    public string AssignedDialogueId;
    public string ChoiceText;
    public string JumpToDialogueId;
}