using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactionService : ScriptableObjectService<FactionSO>
{
    [field: SerializeField] public Transform factionParentTransform { get; private set; }
    [field: SerializeField] public Transform armyParentTransform { get; private set; }
    [field: SerializeField] public GameObject factionRootPrefab { get; private set; }
    [field: SerializeField] public GameObject armyRootPrefab { get; private set; }

    private StructureService structureService;
    private FactionSquadPresetService factionSquadPresetService;
    private RelationshipService relationshipService;

    private void Awake()
    {
        structureService = transform.parent.GetComponentInChildren<StructureService>();
        factionSquadPresetService = transform.parent.GetComponentInChildren<FactionSquadPresetService>();
        relationshipService = transform.parent.GetComponentInChildren<RelationshipService>();
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
            faction.StartCurrencyAmount = data.StartCurrencyAmount;
            faction.MinSquadAmount = data.MinSquadAmount;
            faction.MaxSquadAmount = data.MaxSquadAmount;
            faction.FactionArmySpawnType = Util.ReturnEnumValuesFromStringValues<FactionArmySpawnType>(data.FactionArmySpawnType);
            faction.SquadPresets = factionSquadPresetService.GetFactionSquadPresets(data.Id);

            scriptableObjects.Add(faction);
        }

        base.CreateScriptableObjects();
    }

    public void CreateFactionObjects()
    {
        foreach (FactionSO data in scriptableObjects)
        {
            GameObject obj = Instantiate(factionRootPrefab, factionParentTransform);
            obj.name = data.Name;

            ObjectStorage objectStorage = obj.GetComponent<ObjectStorage>();
            objectStorage.SetObject<FactionSO>(data);

            FactionServiceReference factionServiceReference = obj.GetComponent<FactionServiceReference>();
            factionServiceReference.FactionService = this;

            StructureServiceReference structureServiceReference = obj.GetComponent<StructureServiceReference>();
            structureServiceReference.StructureService = structureService;

            CurrencyStorage currencyStorage = obj.GetComponent<CurrencyStorage>();
            currencyStorage.ModifyCurrency(data.StartCurrencyAmount);

            FactionRelationship factionRelationship = obj.GetComponent<FactionRelationship>();
            factionRelationship.relationships = relationshipService.GetScriptableObject(data.Id).Relationships;
        }
    }

    public Transform GetFactionTransform(string factionId)
    {
        foreach (Transform faction in factionParentTransform)
        {
            if (faction.GetComponent<ObjectStorage>().GetObject<FactionSO>().Id.Equals(factionId))
            {
                return faction;
            }
        }

        Debug.LogError("Couldn't find faction transform where objectStorage FactionSO has ID: " + factionId);

        return null;
    }
}
