using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SquadStorage : MonoBehaviour
{
    public event Action OnNewSquadAdded;

    [field: SerializeField] public List<Squad> Squads { get; private set; }

    private const float SPEED_PENALTY_PER_SOLDIER = 0.025f;

    private void Awake()
    {
        Squads = new();
    }

    public void AddSquad(Squad squad)
    {
        Squads.Add(squad);

        OnNewSquadAdded?.Invoke();
    }

    public int GetTotalSoldierCount()
    {
        return Squads.Sum(squad => squad.GetSoldierCount());
    }

    public float GetSpeedPenalty()
    {
        return GetTotalSoldierCount() * SPEED_PENALTY_PER_SOLDIER;
    }
}
