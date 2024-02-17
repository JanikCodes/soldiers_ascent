using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SquadStorage : MonoBehaviour
{
    [field: SerializeField] public List<Squad> Squads { get; private set; }

    private void Awake()
    {
        Squads = new List<Squad>();
    }

    public void AddSquad(Squad squad)
    {
        Squads.Add(squad);
    }

    public int GetTotalSoldierCount()
    {
        return Squads.Sum(squad => squad.GetSoldierCount());
    }
}
