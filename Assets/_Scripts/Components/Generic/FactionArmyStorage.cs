using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactionArmyStorage : MonoBehaviour
{
    public List<GameObject> Armies { get; set; }

    public int GetArmyCount()
    {
        return Armies.Count;
    }
}
