using System;
using System.Collections;
using System.Collections.Generic;
using RenownedGames.AITree;
using UnityEngine;

[NodeContent("Get Flee Position", "Base/Army/Get Flee Position", IconPath = "Images/Icons/Node/SendMessageIcon.png")]
public class GetFleePositionService : IntervalServiceNode
{
    [Header("Variables")]
    [SerializeField]
    [NonLocal]
    private TransformKey targetToFleeFrom;

    [SerializeField]
    private FloatKey length;

    [SerializeField]
    [NonLocal]
    private Vector3Key outputPosition;

    // Stored required components.
    private Transform transform;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        transform = GetOwner().transform;
    }

    protected override void OnTick()
    {
        Vector3 playerPos = transform.position;
        Vector3 targetPos = targetToFleeFrom.GetValue().position;

        Vector3 fleeDirection = playerPos - targetPos;

        // normalize the flee direction to get a unit vector in that direction
        Vector3 normalizedFleeDirection = fleeDirection.normalized;

        // scale the normalized flee direction to the desired length
        outputPosition.SetValue(playerPos + normalizedFleeDirection * length.GetValue());
    }
}
