using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactionService : ScriptableObjectService<FactionSO>
{
    public static OnNewArmySpawnedDelegate OnNewArmySpawned;
    public delegate void OnNewArmySpawnedDelegate(Transform armyTransform, string factionId);

    [field: SerializeField] public Transform FactionParentTransform { get; private set; }
    [field: SerializeField] public Transform ArmyParentTransform { get; private set; }
    [field: SerializeField] public GameObject FactionRootPrefab { get; private set; }
    [field: SerializeField] public GameObject ArmyRootPrefab { get; private set; }

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
            GameObject obj = Instantiate(FactionRootPrefab, FactionParentTransform);
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
        foreach (Transform faction in FactionParentTransform)
        {
            if (faction.GetComponent<ObjectStorage>().GetObject<FactionSO>().Id.Equals(factionId))
            {
                return faction;
            }
        }

        Debug.LogError("Couldn't find faction transform where objectStorage FactionSO has ID: " + factionId);

        return null;
    }

    public void CreateAndSpawnArmy(Vector3 rawPosition, string factionId, GameObject prefab = null)
    {
        // do we take the default prefab or the optional?
        GameObject selectedPrefab = prefab ? prefab : ArmyRootPrefab;
        // get faction transform based on given factionId
        Transform factionTransform = GetFactionTransform(factionId);

        GameObject army = Instantiate(selectedPrefab, ArmyParentTransform);
        army.transform.position = Util.CalculatePositionOnTerrain(rawPosition);

        // populate components ( when getting components we need to use GetComponentInChildren() due to the possibility of PlayerStructure or maybe create a specific component that points to the armyRoot of the playerStructure )
        FactionAssociation factionAssociation = army.GetComponentInChildren<FactionAssociation>();
        factionAssociation.AssociatedFactionTransform = factionTransform;

        OnNewArmySpawned?.Invoke(army.transform, factionId);
    }
}
