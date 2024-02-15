using System;
using System.Collections.Generic;
using RenownedGames.AITree;
using RenownedGames.Apex;
using UnityEngine;

[NodeContent("Spawn Army", "Tasks/Base/Faction/Spawn Army", IconPath = "Images/Icons/Node/SendMessageIcon.png")]
public class SpawnArmyTask : TaskNode
{
    [Header("Variables")]
    [SerializeField]
    private Vector3Key spawnLocation;
    [SerializeField]
    private IntKey squadAmount;

    // Stored required components.
    private ObjectStorage objectStorage;
    private FactionServiceReference factionServiceReference;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        objectStorage = GetOwner().GetComponent<ObjectStorage>();
        factionServiceReference = GetOwner().GetComponent<FactionServiceReference>();
    }

    protected override void OnEntry()
    {
        base.OnEntry();
    }

    protected override State OnUpdate()
    {
        FactionSO factionData = objectStorage.GetObject<FactionSO>();
        FactionService factionService = factionServiceReference.FactionService;

        if (squadAmount.GetValue() == 0 || !factionService)
        {
            return State.Failure;
        }

        // select random squadPresets
        for (int i = 0; i < squadAmount.GetValue(); i++)
        {
            FactionSquadPresetSO preset = Util.GetRandomValue<FactionSquadPresetSO>(factionData.SquadPresets);

            // TODO: buy preset
            // TODO: reduce faction currency based on presets
        }

        // spawn army
        factionService.CreateAndSpawnArmy(spawnLocation.GetValue(), factionData.Id);

        return State.Success;
    }

    protected override void OnExit()
    {
        base.OnExit();
    }
}