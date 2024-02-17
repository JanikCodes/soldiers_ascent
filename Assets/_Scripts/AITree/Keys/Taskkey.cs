using System.Collections;
using System.Collections.Generic;
using RenownedGames.AITree;
using UnityEngine;

[KeyColor(0.231f, 0.5f, 0.27f)]
public class TaskKey : Key<Task>
{
    public override bool Equals(Task other)
    {
        return GetValue() == other;
    }
}