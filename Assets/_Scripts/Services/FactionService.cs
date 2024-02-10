using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactionService : ScriptableObjectService<FactionSO>
{
    [SerializeField] private Transform factionParentTransform;
    [SerializeField] private GameObject objectPrefab;

    private StructureService structureService;
    private FactionSquadPresetService factionSquadPresetService;

    private void Awake()
    {
        structureService = transform.parent.GetComponentInChildren<StructureService>();
        factionSquadPresetService = transform.parent.GetComponentInChildren<FactionSquadPresetService>();
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
    }

    public override void CreateScriptableObjects()
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
            faction.SquadPresets = factionSquadPresetService.GetFactionSquadPresets(data.Id);

            scriptableObjects.Add(faction);
        }

        base.CreateScriptableObjects();
    }
}
