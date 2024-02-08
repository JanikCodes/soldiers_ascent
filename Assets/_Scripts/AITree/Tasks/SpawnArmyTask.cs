using System;
using System.Collections.Generic;
using RenownedGames.AITree;
using RenownedGames.Apex;
using UnityEngine;

[NodeContent("CreateArmy", "Tasks/Base/Faction/CreateArmy", IconPath = "Images/Icons/Node/WaitIcon.png")]
public class CreateArmyTask : TaskNode
{
    // Stored required components.
    private FactionSO factionData;
    private FactionArmyStorage factionArmyStorage;

    protected override void OnInitialize()
    {
        base.OnInitialize();
    }

    protected override void OnEntry()
    {
        base.OnEntry();

        factionArmyStorage = GetOwner().GetComponent<FactionArmyStorage>();
    }

    protected override State OnUpdate()
    {
        FactionArmySpawnType spawnType = Util.GetRandomValue<FactionArmySpawnType>(factionData.FactionArmySpawnType);
        
        switch(spawnType)
        {
            case FactionArmySpawnType.OwnedStructures:
                SpawnArmyAtOwnedStructures();
            break;
            default:
                Debug.LogWarning($"Spawntype '{spawnType}' Not implemented yet!");
            break;
        }

        return State.Success;
    }

    private void SpawnArmyAtOwnedStructures()
    {

    }

    protected override void OnExit()
    {
        base.OnExit();
    }
}