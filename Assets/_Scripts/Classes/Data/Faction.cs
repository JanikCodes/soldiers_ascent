using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that is used to generate the ScriptableObject <see cref="FactionSO"/> at runtime from.
/// </summary>
[Serializable]
public class FactionData : BaseData
{
    public string Name;
    public string Description;
    public bool Visible;
    public int MaxArmyCountOnOverworld;
    public int StartCurrencyAmount;
    public int MinSquadAmount;
    public int MaxSquadAmount;
    public string[] FactionArmySpawnType;
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