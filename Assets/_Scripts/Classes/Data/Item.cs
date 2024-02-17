using System;

[Serializable]
public class ItemData : BaseData
{
    public string Name;
    public string Description;
    public string ItemType;
    public int BaseValue;
    public string[] PossibleRarities;
    public int FoodValue;
}