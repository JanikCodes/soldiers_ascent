using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RelationshipData : BaseData
{
    public List<RelationshipConnectionData> Relationships;

    [Serializable]
    public struct RelationshipConnectionData
    {
        public string FactionId;
        public int RelationshipValue;
    }
}

/// <summary>
/// DataContainer class for <see cref="RelationshipData"/> saving dynamic data
/// </summary>
[Serializable]
public class RelationshipSaveData
{
    // TODO: . . .
    // . . .
}