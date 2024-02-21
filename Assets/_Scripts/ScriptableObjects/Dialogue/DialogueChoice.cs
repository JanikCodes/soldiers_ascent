using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueChoiceSO : DataSO
{
    [NonSerialized] public DialogueSO JumpToDialogue;
    public string ChoiceText;
    public string rawJumpToDialogueId { get; set; }
}
