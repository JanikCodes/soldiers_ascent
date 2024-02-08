using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactionService : MonoBehaviour
{
    [SerializeField] private Transform factionParentTransform;
    [SerializeField] private GameObject objectPrefab;
    [SerializeField] private List<FactionSO> scriptableObjects = new List<FactionSO>();

    private StructureService structureService;

    private void Awake()
    {
        structureService = transform.parent.GetComponentInChildren<StructureService>();
    }

    public void CreateFactionObjects()
    {
        foreach (FactionSO data in scriptableObjects)
        {
            GameObject obj = Instantiate(objectPrefab, factionParentTransform);
            obj.name = data.Name;

            ObjectStorage objectStorage = obj.GetComponent<ObjectStorage>();
            objectStorage.SetObject<FactionSO>(data);

            FactionServiceReference factionServiceReference = obj.GetComponent<FactionServiceReference>();
            factionServiceReference.FactionService = this;

            StructureServiceReference structureServiceReference = obj.GetComponent<StructureServiceReference>();
            structureServiceReference.StructureService = structureService;

            CurrencyStorage currencyStorage = obj.GetComponent<CurrencyStorage>();
            currencyStorage.ModifyCurrency(data.StartCurrencyAmount);
        }

        Debug.Log("Finished creating faction objects");
    }

    public void CreateScriptableObjects()
    {
        List<FactionData> rawData = DataService.CreateDataFromFilesAndMods<FactionData>("Factions");

        foreach (FactionData data in rawData)
        {
            if (!data.Active) { continue; }

            FactionSO faction = ScriptableObject.CreateInstance<FactionSO>();
            faction.name = data.Id;
            faction.Id = data.Id;
            faction.Name = data.Name;
            faction.Visible = data.Visible;
            faction.Description = data.Description;
            faction.MaxArmyCountOnOverworld = data.MaxArmyCountOnOverworld;
            faction.SpawnArmyInterval = data.SpawnArmyInterval;
            faction.StartCurrencyAmount = data.StartCurrencyAmount;
            faction.FactionArmySpawnType = Util.ReturnEnumValuesFromStringValues<FactionArmySpawnType>(data.FactionArmySpawnType);

            scriptableObjects.Add(faction);
        }

        Debug.Log("Finished adding faction scriptableobjects");
    }

    internal FactionSO GetFactionSOByString(string id)
    {
        foreach (FactionSO faction in scriptableObjects)
        {
            if (faction.Id.Equals(id))
            {
                return faction;
            }
        }

        return null;
    }
}
