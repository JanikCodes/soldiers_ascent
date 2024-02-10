using System;
using System.Collections.Generic;
using RenownedGames.AITree;
using RenownedGames.Apex;
using UnityEngine;

[NodeContent("Spawn Army", "Tasks/Base/Faction/Spawn Army", IconPath = "Images/Icons/Node/WaitIcon.png")]
public class SpawnArmyTask : TaskNode
{
    public static OnNewArmySpawnedDelegate OnNewArmySpawned;
    public delegate void OnNewArmySpawnedDelegate(Transform armyTransform, string factionId);

    [Header("Variables")]
    [SerializeField]
    private TransformKey spawnLocation;
    [SerializeField]
    private IntKey squadAmount;

    // Stored required components.
    private FactionSO factionData;
    private FactionArmyReference factionArmyReference;
    private FactionService factionService;

    protected override void OnInitialize()
    {
        base.OnInitialize();
    }

    protected override void OnEntry()
    {
        base.OnEntry();

        factionData = GetOwner().GetComponent<ObjectStorage>().GetObject<FactionSO>();
        factionArmyReference = GetOwner().GetComponent<FactionArmyReference>();
        factionService = GetOwner().GetComponent<FactionServiceReference>().FactionService;
    }

    protected override State OnUpdate()
    {
        if (squadAmount.GetValue() == 0)
        {
            return State.Failure;
        }

        // select random squadPresets
        for (int i = 0; i < squadAmount.GetValue(); i++)
        {
            FactionSquadPresetSO preset = Util.GetRandomValue<FactionSquadPresetSO>(factionData.SquadPresets);

            // TODO: buy preset
            // TODO: reduce faction currency based on presets
            // TODO: create army prefab
            // TODO: position it at spawnLocation
            // TODO: populate it properly
        }

        GameObject army = Instantiate(factionService.armyRootPrefab, factionService.armyParentTransform);
        army.transform.position = spawnLocation.GetValue().position;

        OnNewArmySpawned?.Invoke(army.transform, factionData.Id);

        return State.Success;
    }

    protected override void OnExit()
    {
        base.OnExit();
    }
}