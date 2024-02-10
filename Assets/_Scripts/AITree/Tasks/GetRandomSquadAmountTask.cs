using System.Collections;
using System.Collections.Generic;
using RenownedGames.AITree;
using UnityEngine;

[NodeContent("Get Random Squad Amount", "Tasks/Base/Faction/Get Random Squad Amount", IconPath = "Images/Icons/Node/WaitIcon.png")]
public class GetRandomSquadAmountTask : TaskNode
{
    [Header("Variables")]
    [SerializeField]
    [NonLocal]
    private IntKey outputSquadAmount;

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
        int squadAmount = Util.GetRandomValue(factionData.MinSquadAmount, factionData.MaxSquadAmount);
        if(squadAmount < 0)
        {
            return State.Failure;
        }

        outputSquadAmount.SetValue(squadAmount);

        return State.Success;
    }

    protected override void OnExit()
    {
        base.OnExit();
    }
}
