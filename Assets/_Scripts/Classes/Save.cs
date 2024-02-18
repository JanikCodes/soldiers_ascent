using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Save
{
    public float Version;
    public long LastTimePlayed;
    public List<FactionSaveData> Factions = new();
    public List<StructureSaveData> Structures = new();
}