using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureBuildingService : ScriptableObjectService<StructureBuildingSO>
{
    private BuildingService buildingService;

    private void Awake()
    {
        buildingService = GetOtherService<BuildingService>();
    }

    public override void CreateScriptableObjects()
    {
        List<StructureBuildingData> rawData = DataService.CreateDataFromFilesAndMods<StructureBuildingData>("StructureBuildings");

        foreach (StructureBuildingData data in rawData)
        {
            if (!data.Active) { continue; }

            StructureBuildingSO structureBuilding = ScriptableObject.CreateInstance<StructureBuildingSO>();
            structureBuilding.name = data.Id;
            structureBuilding.Id = data.Id;
            structureBuilding.StructureId = data.StructureId;
            structureBuilding.BuildingId = data.BuildingId;
            structureBuilding.AutoBuildAtStart = data.AutoBuildAtStart;

            scriptableObjects.Add(structureBuilding);
        }

        base.CreateScriptableObjects();
    }

    /// <summary>
    /// Receives all possible buildings for a structure.
    /// </summary>
    public List<BuildingSO> GetPossibleStructureBuildings(string structureId)
    {
        List<BuildingSO> buildings = new();
        List<StructureBuildingSO> structureBuildings = scriptableObjects.FindAll(data => data.StructureId.Equals(structureId));
        foreach (StructureBuildingSO structureBuilding in structureBuildings)
        {
            BuildingSO buildingSO = buildingService.GetScriptableObject(structureBuilding.BuildingId);
            if (buildingSO == null)
            {
                Debug.LogWarning("Unable to find building with ID: " + structureBuilding.BuildingId);
                continue;
            }

            buildings.Add(buildingSO);
        }

        return buildings;
    }
}
