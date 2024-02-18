using System;
using System.Collections.Generic;

/// <summary>
/// DataContainer class dependency for <see cref="FactionData"/> saving dynamic data
/// </summary>
[Serializable]
public class ArmySaveData
{
    public string GUID;
    public float[] Position;
    public float[] Rotation;
    public List<SquadSaveData> Squads = new();
}