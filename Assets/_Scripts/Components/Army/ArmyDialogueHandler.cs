using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class ArmyDialogueHandler : MonoBehaviour, IDialogueHandler
{
    [Tooltip("True if a dialogue is actively running.")]
    [SerializeField, ReadOnlyField] private bool active;

    public void BeingTalkedTo(Transform other)
    {
        if (active) { return; }

        active = true;

        Debug.Log("AI is being talked by ... " + other.name);
    }

    public bool IsInDialogue()
    {
        return active;
    }

    public void ProcessDialogue()
    {
        // empty
        // TODO: based on type, do nothing or calculate battle result
    }

    public void TalkTo(Transform other)
    {
        if (active) { return; }

        IDialogueHandler otherDialogueHandler = other.GetComponent<IDialogueHandler>();
        if (otherDialogueHandler == null)
        {
            Debug.LogWarning("Couldn't instantiate dialogue because the other is missing a component that is inheriting from IDialogueHandler.");
            return;
        }

        // is other not already in dialogue?
        if (otherDialogueHandler.IsInDialogue()) { return; }

        // notify other that we're talking to him
        otherDialogueHandler.BeingTalkedTo(transform);

        // set states
        active = true;

        Debug.Log("AI is talking to ... " + other.name);
    }
}
