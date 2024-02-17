using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemSO : DataSO
{
    public string Name;
    public string Description;
    public ItemType ItemType;
    public int BaseValue;
    public RarityType[] PossibleRarities;
    public int FoodValue;
}
