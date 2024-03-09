using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SquadStorage : MonoBehaviour
{
    public event Action OnNewSquadAdded;
    public event Action OnSoldierRemoved;

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

    /// <summary>
    /// Remove a random soldier from a random squad
    /// </summary>
    /// <returns>True if it successfully removed a soldier.</returns>
    public bool RemoveRandomSoldier()
    {
        foreach (Squad squad in Squads)
        {
            if (squad.GetSoldierCount() > 0)
            {
                Soldier randomSoldier = Util.GetRandomValue<Soldier>(squad.GetSoldiers());
                squad.RemoveSoldier(randomSoldier);

                OnSoldierRemoved?.Invoke();
                return true;
            }
        }

        return false;
    }

    public float GetSpeedPenalty()
    {
        return GetTotalSoldierCount() * SPEED_PENALTY_PER_SOLDIER;
    }
}
