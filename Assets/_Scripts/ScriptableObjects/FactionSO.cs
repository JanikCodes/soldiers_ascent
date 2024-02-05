using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactionSO : DataSO
{
    public string Name;
    public string Description;
    public bool Visible;
    public int MaxArmyCountOnOverworld;
    public int SpawnArmyInterval;
    public int StartCurrencyAmount;
    public GameObject ArmyPrefab; // Temporary prefab
}
