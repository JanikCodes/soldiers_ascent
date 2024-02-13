using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

[RequireComponent(typeof(IAstarAI))]
public class WorldPlayerMovement : MonoBehaviour
{
    private WorldPlayerInput input;

    private void Awake()
    {
        input = WorldPlayerInput.Instance;
    }
}
