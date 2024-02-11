using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class AIPositionSetter : MonoBehaviour
{
    [field: SerializeField] public Vector3 Position { get; set; }

    private IAstarAI ai;

    private void OnEnable()
    {
        ai = GetComponent<IAstarAI>();

        if (ai != null)
        {
            ai.onSearchPath += UpdatePosition;
        }
    }

    private void OnDisable()
    {
        if (ai != null)
        {
            ai.onSearchPath -= UpdatePosition;
        }
    }

    private void Start()
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        if (ai != null)
        {
            ai.destination = Position;
        }
    }
}
