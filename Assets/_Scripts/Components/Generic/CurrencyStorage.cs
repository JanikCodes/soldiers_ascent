using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Component that can be attached to Gameobjects in order to store currency. Be it a destroyable prop or an army.
/// </summary>
public class CurrencyStorage : MonoBehaviour
{
    [field: SerializeField] public int Currency { get; private set; }

    public bool HasEnoughCurrency(int value)
    {
        return Currency >= value;
    }

    public void ModifyCurrency(int value)
    {
        Currency += value;
    }
}
