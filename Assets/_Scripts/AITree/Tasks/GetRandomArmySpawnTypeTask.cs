using System.Collections;
using System.Collections.Generic;
using RenownedGames.AITree;
using RenownedGames.Apex;
using UnityEngine;

[NodeContent("Get Random Army Spawn Type", "Tasks/Base/Faction/Get Random Army Spawn Type", IconPath = "Images/Icons/Node/WaitIcon.png")]
public class GetRandomArmySpawnTypeTask : TaskNode
{
    [SerializeField]
    [NonLocal]
    private FactionArmySpawnTypeKey outputArmySpawnType;

    // Stored required components.
    private FactionSO factionData;

    protected override void OnInitialize()
    {
        base.OnInitialize();
    }

    protected override void OnEntry()
    {
        base.OnEntry();

        factionData = GetOwner().GetComponent<ObjectStorage>().GetObject<FactionSO>();
    }

    protected override State OnUpdate()
    {
        if (factionData.FactionArmySpawnType.Length == 0)
        {
            return State.Failure;
        }

        outputArmySpawnType.SetValue(Util.GetRandomValue<FactionArmySpawnType>(factionData.FactionArmySpawnType));

        return State.Success;
    }

    protected override void OnExit()
    {
        base.OnExit();
    }
}