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

    private Transform dialoguePartner;

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
    public void InstantiateDialogue(Transform dialoguePartner)
    {
        active = true;
        Debug.Log("Instantiating dialogue with " + dialoguePartner.name);
    }

    private bool ShouldTriggerDialogue()
    {
        return Vector3.Distance(aIDestinationSetter.target.position, transform.position) <= dialogueTriggerDistance;
    }
}
