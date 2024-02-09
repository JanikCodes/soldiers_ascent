using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

/// <summary>
/// Component that can be attached to Gameobjects in order to have a reference to a <see cref="FactionSO"/>
/// </summary>
public class FactionAssociation : MonoBehaviour
{   
    [field: SerializeField, ReadOnlyField] public FactionSO Associated { get; set; }
}
