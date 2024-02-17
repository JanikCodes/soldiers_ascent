using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that is used to generate the ScriptableObject <see cref="StructureSO"/> at runtime from.
/// </summary>
[Serializable]
public class StructureData : BaseData
{
    public string Name;
    public string Description;
    public float[] Position;
    public float Rotation;
    public string OwnedByFactionId;
    public string AssignedPrefabId;
}

/// <summary>
/// DataContainer class for <see cref="StructureData"/> saving dynamic data
/// </summary>
[Serializable]
public class StructureSaveData
{
    public string Id;
    public string OwnedByFactionId;
}