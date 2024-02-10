using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureService : MonoBehaviour
{
    [SerializeField] private Transform structureParentTransform;
    [SerializeField] private GameObject objectPrefab;
    [SerializeField] private List<StructureSO> scriptableObjects = new List<StructureSO>();

    private FactionService factionService;

    private void Awake()
    {
        factionService = transform.parent.GetComponentInChildren<FactionService>();
    }

    public void CreateStructureObjects()
    {
        foreach (StructureSO data in scriptableObjects)
        {
            GameObject obj = Instantiate(objectPrefab, structureParentTransform);
            obj.name = data.Name;
            obj.transform.localScale = new Vector3(1, 1, 1);

            Vector3 calculatedPosition = CalculatePositionOnDynamicTerrain(data);

            obj.transform.rotation = data.Rotation;
            obj.transform.position = calculatedPosition;

            FactionAssociation factionAssociation = obj.GetComponent<FactionAssociation>();
            factionAssociation.Associated = factionService.GetFactionSOById(data.InitiallyOwnedByFaction);
        }

        Debug.Log("Finished creating structure objects");
    }

    public void CreateScriptableObjects()
    {
        List<StructureData> rawData = DataService.CreateDataFromFilesAndMods<StructureData>("Structures");

        foreach (StructureData data in rawData)
        {
            if (!data.Active) { continue; }

            StructureSO structure = ScriptableObject.CreateInstance<StructureSO>();
            structure.name = data.Id;
            structure.Id = data.Id;
            structure.Name = data.Name;
            structure.Description = data.Description;
            structure.Position = new Vector3(data.Position[0], data.Position[1], data.Position[2]);
            structure.Rotation = Quaternion.Euler(0, data.Rotation, 0);
            structure.InitiallyOwnedByFaction = data.OwnedByFactionId;
            // structure.AssignedPrefab = modelCore.GetPrefabFromId(data.AssignedPrefabId);

            scriptableObjects.Add(structure);
        }

        Debug.Log("Finished adding faction scriptableobjects");
    }

    public List<Transform> GetFactionOwnedStructures(FactionSO faction)
    {
        List<Transform> result = new List<Transform>();

        foreach (Transform structure in structureParentTransform)
        {
            FactionAssociation factionAssociation = structure.GetComponent<FactionAssociation>();

            if (factionAssociation.Associated.Id.Equals(faction.Id))
            {
                result.Add(structure);
            }
        }

        return result;
    }

    public List<Transform> GetAllStructures()
    {
        List<Transform> result = new List<Transform>();

        foreach (Transform structure in structureParentTransform)
        {
            result.Add(structure);
        }

        return result;
    }

    private Vector3 CalculatePositionOnDynamicTerrain(StructureSO data)
    {
        float terrainHeightPosition = Terrain.activeTerrain.SampleHeight(data.Position);
        return new Vector3(data.Position.x, terrainHeightPosition, data.Position.z);
    }
}
