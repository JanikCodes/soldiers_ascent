using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

[RequireComponent(typeof(IAstarAI))]
public class ArmySpeed : MonoBehaviour
{
    [SerializeField] private float BaseSpeed;

    [Header("Debugging")]
    [SerializeField, ReadOnlyField] private float squadStoragePenalty = 0f;
    [SerializeField, ReadOnlyField] private float inventoryPenalty = 0f;

    private IAstarAI ai;
    private SquadStorage squadStorage;
    private Inventory inventory;

    private void Awake()
    {
        ai = GetComponent<IAstarAI>();
        squadStorage = GetComponent<SquadStorage>();
        inventory = GetComponent<Inventory>();

        HandleSpeedChange();
    }

    private void OnEnable()
    {
        inventory.OnNewItemAdded += HandleSpeedChange;
        squadStorage.OnNewSquadAdded += HandleSpeedChange;
    }

    private void OnDisable()
    {
        inventory.OnNewItemAdded -= HandleSpeedChange;
        squadStorage.OnNewSquadAdded -= HandleSpeedChange;
    }

    /// <summary>
    /// Is called whenever something changes that could affect speed.
    /// </summary>
    private void HandleSpeedChange()
    {
        if (squadStorage != null)
        {
            squadStoragePenalty = squadStorage.GetSpeedPenalty();
        }

        if (inventory != null)
        {
            inventoryPenalty = inventory.GetSpeedPenalty();
        }

        float calculatedSpeed = BaseSpeed - squadStoragePenalty - inventoryPenalty;

        ai.maxSpeed = Mathf.Max(1, calculatedSpeed);
    }
}
