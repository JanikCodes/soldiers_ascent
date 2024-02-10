using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierService : MonoBehaviour
{
    [SerializeField] private List<SoldierSO> scriptableObjects = new List<SoldierSO>();

    public void CreateScriptableObjects()
    {
        List<SoldierData> rawData = DataService.CreateDataFromFilesAndMods<SoldierData>("Soldiers");

        foreach (SoldierData data in rawData)
        {
            if (!data.Active) { continue; }

            SoldierSO soldier = ScriptableObject.CreateInstance<SoldierSO>();
            soldier.name = data.Id;
            soldier.Id = data.Id;
            soldier.Name = data.Name;
            soldier.Description = data.Description;
            soldier.MovementSpeed = data.MovementSpeed;
            soldier.Health = data.Health;

            scriptableObjects.Add(soldier);
        }

        Debug.Log("Finished adding soldier scriptableobjects");
    }

    public SoldierSO GetSoldierSOById(string id)
    {
        return scriptableObjects.Find(x => x.Id.Equals(id));
    }
}
