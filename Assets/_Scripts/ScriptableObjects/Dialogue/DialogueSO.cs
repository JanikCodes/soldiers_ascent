using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueSO : DataSO
{
    public List<DialogueChoiceSO> Choices = new();

    public List<DialogueChoiceSO> GetChoices()
    {
        List<DialogueChoiceSO> choices = new List<DialogueChoiceSO>();

        foreach (DialogueChoiceSO choice in Choices)
        {
            choices.Add(choice);
        }

        return choices;
    }
}
