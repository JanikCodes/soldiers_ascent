using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

[RequireComponent(typeof(AIDestinationSetter))]
public class DialogueHandler : MonoBehaviour
{
    [SerializeField] private float dialogueTriggerDistance;
    [Tooltip("True if a dialogue is actively running.")]
    [SerializeField, ReadOnlyField] private bool active;

    private AIDestinationSetter aIDestinationSetter;

    private DialogueSO dialogue;

    // events
    public static event OnDialogueInstantiatedDelegate OnDialogueInstantiated;
    public delegate void OnDialogueInstantiatedDelegate(Transform other);

    private void Awake()
    {
        aIDestinationSetter = GetComponent<AIDestinationSetter>();
    }

    private void Update()
    {
        if (active) { return; }
        if (!aIDestinationSetter.target) { return; }

        bool trigger = ShouldTriggerDialogue();

        if (trigger)
        {
            InstantiateDialogue(aIDestinationSetter.target);
        }
    }

    /// <summary>
    /// This methode can be executed by the player or from other armies as well.
    /// </summary>
    public void InstantiateDialogue(Transform other)
    {
        if (active) { return; }

        DialogueTrigger dialogueTrigger = other.GetComponent<DialogueTrigger>();
        if (!dialogueTrigger)
        {
            Debug.LogWarning("Couldn't instantiate dialogue because DialogueTrigger is missing on the partner.");
            return;
        }

        // invoke event to notify subscribers
        OnDialogueInstantiated?.Invoke(other);

        active = true;
        dialogue = dialogueTrigger.Dialogue;

        Debug.Log("Instantiating dialogue with " + other.name);

        List<DialogueChoiceSO> choices = dialogue.GetChoices(transform, other);
        Debug.Log("### CHOICES ###");
        foreach (DialogueChoiceSO choice in choices)
        {
            Debug.Log(choice.ChoiceText);
        }

        Debug.Log("### STOP CHOICES ###");
    }

    private bool ShouldTriggerDialogue()
    {
        return Vector3.Distance(aIDestinationSetter.target.position, transform.position) <= dialogueTriggerDistance;
    }
}
