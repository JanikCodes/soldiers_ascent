using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactionArmyReference : MonoBehaviour
{
    [SerializeField] private List<Transform> referencedArmies = new List<Transform>();

    private ObjectStorage objectStorage;

    private void Awake()
    {
        objectStorage = GetComponent<ObjectStorage>();
    }

    private void OnEnable()
    {
        FactionService.OnNewArmySpawned += ArmySpawnedHandler;

    }

    private void OnDisable()
    {
        FactionService.OnNewArmySpawned -= ArmySpawnedHandler;
    }

    public int GetArmyCount()
    {
        return referencedArmies.Count;
    }

    private void ArmySpawnedHandler(Transform armyTransform, string factionId)
    {
        if (objectStorage.GetObject<FactionSO>().Id.Equals(factionId))
        {
            referencedArmies.Add(armyTransform);

            Debug.Log("Added new army reference to referencedArmies");
        }
    }

    private void ArmyDestroyedHandler(Transform armyTransform, string factionId)
    {
        if (objectStorage.GetObject<FactionSO>().Id.Equals(factionId))
        {
            referencedArmies.Remove(armyTransform);

            Debug.Log("Removed army reference from referencedArmies");
        }
    }
}
