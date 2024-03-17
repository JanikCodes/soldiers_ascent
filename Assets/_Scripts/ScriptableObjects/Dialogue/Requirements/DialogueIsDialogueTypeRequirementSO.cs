using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogueIsDialogueTypeRequirementSO : DialogueRequirementSO
{
    public string Type;

    public override bool CheckRequirements(Transform self, Transform other)
    {
        Transform target = Self ? self : other;

        IDialogueHandler dialogueHandler = target.GetComponent<IDialogueHandler>();
        return dialogueHandler.GetDialogueType().ToString().Equals(Type);
    }
}
