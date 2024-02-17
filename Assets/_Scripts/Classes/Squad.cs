using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Squad
{
    [SerializeField] private List<Soldier> soldiers = new List<Soldier>();

    public void AddSoldier(Soldier soldier)
    {
        soldiers.Add(soldier);
    }
}
