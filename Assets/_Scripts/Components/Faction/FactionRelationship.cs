using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static RelationshipData;

public class FactionRelationship : MonoBehaviour
{
    [field: SerializeField] public List<RelationshipConnectionData> relationships { private get; set; }

    public void AlterRelationshipWithFaction(string factionId, int relationValue)
    {
        RelationshipConnectionData found = relationships.Find(x => x.FactionId.Equals(factionId));
        found.RelationshipValue += relationValue;
    }

    public int GetRelationshipValueTowardsFaction(string factionId)
    {
        return relationships.Find(x => x.FactionId.Equals(factionId)).RelationshipValue;
    }
}
