using System;
using UnityEngine;

/// <summary>
/// Class that is used to generate the ScriptableObject <see cref="ItemSO"/> at runtime from.
/// </summary>
[Serializable]
public class ItemData : BaseData
{
    public string Name;
    public string Description;
    public string ItemType;
    public int BaseValue;
    public int MaxStackSize;
    public string[] PossibleRarities;
    public int FoodValue;
}

[Serializable]
public class Item
{   
    public ItemData ItemBaseData;

    [Header("Dynamic Data")]
    public int Count;
    public int SlotIndex;

    public object Clone()
    {
        return MemberwiseClone();
    }
}