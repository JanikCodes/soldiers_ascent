using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FactionData : BaseData
{
    public string Name;
    public string Description;
    public bool Visible;
    public int MaxArmyCountOnOverworld;
    public int SpawnArmyInterval;
    public int StartCurrencyAmount;
}

/// <summary>
/// DataContainer class for <see cref="FactionData"/> saving dynamic data
/// </summary>
[Serializable]
public class FactionSaveData
{
    public string Id;
    public int CurrencyAmount;
    // . . .
}