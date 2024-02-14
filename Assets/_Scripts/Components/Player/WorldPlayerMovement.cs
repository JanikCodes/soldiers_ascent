using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

[RequireComponent(typeof(IAstarAI))]
public class WorldPlayerMovement : MonoBehaviour
{
    private IAstarAI ai;
    private WorldPlayerInput input;

    private void Awake()
    {
        input = WorldPlayerInput.Instance;

        ai = GetComponent<IAstarAI>();
    }

    private void OnEnable()
    {
        input.OnSingleClick += SingleMouseClick;
    }

    private void OnDisable()
    {
        input.OnSingleClick -= SingleMouseClick;
    }

    private void SingleMouseClick()
    {
        // TODO: check here if we can even move? Or do that elsewhere? perhaps I shoot an event and try listening for a callback or smth? So any script that does not want me to be able to move can return false or smth?

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(input.GetMousePosition());
        if (Physics.Raycast(ray, out hit, 1000))
        {
            SetDestination(hit.point);
        }
    }

    private void SetDestination(Vector3 position)
    {
        ai.destination = position;
    }
}
