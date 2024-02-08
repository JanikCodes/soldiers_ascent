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
    private FactionArmyReference factionArmyReference;
    private StructureServiceReference structureService;

    protected override void OnInitialize()
    {
        base.OnInitialize();
    }

    protected override void OnEntry()
    {
        base.OnEntry();

        factionData = GetOwner().GetComponent<ObjectStorage>().GetObject<FactionSO>();
        factionArmyReference = GetOwner().GetComponent<FactionArmyReference>();
        structureService = GetOwner().GetComponent<StructureServiceReference>();
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
        List<GameObject> structures = structureService.StructureService.GetFactionOwnedStructures(factionData);
        if(structures.Count == 0)
        {
            Debug.LogWarning("Faction cannot spawn army at owned structures because structure count is 0");
            return;
        }

        GameObject selectedStructure = Util.GetRandomValue<GameObject>(structures);

        Debug.Log($"Spawning army at: {selectedStructure.gameObject.name} ");
    }

    protected override void OnExit()
    {
        base.OnExit();
    }
}