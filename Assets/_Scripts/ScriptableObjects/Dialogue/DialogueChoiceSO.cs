using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueChoiceSO : DataSO
{
    [NonSerialized] public DialogueSO JumpToDialogue;
    public string ChoiceText;
    public string RawJumpToDialogueId;
    public List<DialogueRequirementSO> Requirements = new();

    public bool MeetsRequirements(Transform self, Transform other)
    {
        bool valid = true;

        // Check if any requirement is false
        foreach (DialogueRequirementSO requirementSO in Requirements)
        {
            if (!requirementSO.CheckRequirements(self, other))
            {
                valid = false;
                break;
            }
        }

        return valid;
    }
}
