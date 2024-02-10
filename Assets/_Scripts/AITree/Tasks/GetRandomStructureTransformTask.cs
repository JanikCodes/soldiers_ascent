using System.Collections;
using System.Collections.Generic;
using RenownedGames.AITree;
using UnityEngine;

[NodeContent("Get Random Structure Transform", "Tasks/Base/Faction/Get Random Structure Transform", IconPath = "Images/Icons/Node/WaitIcon.png")]
public class GetRandomStructureTransformTask : TaskNode
{
    [SerializeField]
    private BoolKey factionOwnedStructuresOnly;

    [SerializeField]
    [NonLocal]
    private TransformKey outputTransform;

    // Stored required components.
    private FactionSO factionData;
    private StructureServiceReference structureService;

    protected override void OnInitialize()
    {
        base.OnInitialize();
    }

    protected override void OnEntry()
    {
        base.OnEntry();

        factionData = GetOwner().GetComponent<ObjectStorage>().GetObject<FactionSO>();
        structureService = GetOwner().GetComponent<StructureServiceReference>();
    }

    protected override State OnUpdate()
    {
        List<Transform> structures;

        if (factionOwnedStructuresOnly)
        {
            structures = structureService.StructureService.GetFactionOwnedStructures(factionData);
        }
        else
        {
            structures = structureService.StructureService.GetAllStructures();
        }

        if (structures.Count == 0)
        {
            return State.Failure;
        }

        outputTransform.SetValue(Util.GetRandomValue<Transform>(structures));

        return State.Success;
    }

    protected override void OnExit()
    {
        base.OnExit();
    }
}