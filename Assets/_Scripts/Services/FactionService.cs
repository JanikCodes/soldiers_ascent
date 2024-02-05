using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactionService : MonoBehaviour
{
    [SerializeField] private List<FactionSO> scriptableObjects = new List<FactionSO>();

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
            // faction.ArmyPrefab = armyPrefab;

            scriptableObjects.Add(faction);
        }

        Debug.Log("Finished adding faction scriptableobjects");
    }
}
