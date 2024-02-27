using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

[RequireComponent(typeof(AIDestinationSetter))]
public class PlayerDialogueHandler : MonoBehaviour, IDialogueHandler
{
    [SerializeField] private float dialogueTriggerDistance;
    [Tooltip("True if a dialogue is actively running.")]
    [SerializeField, ReadOnlyField] private bool active;

    private AIDestinationSetter aIDestinationSetter;
    private TimeServiceReference timeServiceReference;

    private DialogueSO dialogue;

    // events
    public static event OnDialogueInstantiatedDelegate OnDialogueInstantiated;
    public delegate void OnDialogueInstantiatedDelegate(Transform other);

    private void Awake()
    {
        aIDestinationSetter = GetComponent<AIDestinationSetter>();
        timeServiceReference = GetComponent<TimeServiceReference>();
    }

    private void Update()
    {
        if (active) { return; }
        if (!aIDestinationSetter.target) { return; }

        bool trigger = ShouldTriggerDialogue();

        if (trigger)
        {
            TalkTo(aIDestinationSetter.target);
        }
    }

    public void ProcessDialogue()
    {
        // empty
    }

    public void BeingTalkedTo(Transform other)
    {
        if(active) { return; }
        
        DialogueTrigger dialogueTrigger = other.GetComponent<DialogueTrigger>();
        if (!dialogueTrigger)
        {
            Debug.LogWarning("Couldn't instantiate dialogue because DialogueTrigger is missing on the partner.");
            return;
        }

        if (!dialogueTrigger.Dialogue)
        {
            Debug.LogWarning("Couldn't instantiate dialogue because the dialogue is missing on the partner.");
            return;
        }

        // notify subscribers and quest progress
        OnDialogueInstantiated?.Invoke(other);

        // pause time hard till dialogue is resolved
        timeServiceReference.Service.SetTime(TimeState.Paused, true);

        // set states
        active = true;
        dialogue = dialogueTrigger.Dialogue;

        Debug.Log("Player is being talked to by ... " + other.name);
    }

    public bool IsInDialogue()
    {
        return active;
    }

    /// <summary>
    /// This methode is ONLY executed by the player.
    /// </summary>
    public void TalkTo(Transform other)
    {
        if (active) { return; }

        DialogueTrigger dialogueTrigger = other.GetComponent<DialogueTrigger>();
        if (!dialogueTrigger)
        {
            Debug.LogWarning("Couldn't instantiate dialogue because DialogueTrigger is missing on the partner.");
            return;
        }

        if (!dialogueTrigger.Dialogue)
        {
            Debug.LogWarning("Couldn't instantiate dialogue because the dialogue is missing on the partner.");
            return;
        }

        IDialogueHandler otherDialogueHandler = other.GetComponent<IDialogueHandler>();
        if (otherDialogueHandler == null)
        {
            Debug.LogWarning("Couldn't instantiate dialogue because the other is missing a component that is inheriting from IDialogueHandler.");
            return;
        }

        // notify other that we're talking to him
        otherDialogueHandler.BeingTalkedTo(transform);

        // notify subscribers and quest progress
        OnDialogueInstantiated?.Invoke(other);

        // pause time hard till dialogue is resolved
        timeServiceReference.Service.SetTime(TimeState.Paused, true);

        // set states
        active = true;
        dialogue = dialogueTrigger.Dialogue;

        Debug.Log("Player is talking to ... " + other.name);
    }

    private bool ShouldTriggerDialogue()
    {
        return Vector3.Distance(aIDestinationSetter.target.position, transform.position) <= dialogueTriggerDistance;
    }
}
