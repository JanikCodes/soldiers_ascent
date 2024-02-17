using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemService : ScriptableObjectService<ItemSO>
{
    public override void CreateScriptableObjects()
    {
        List<ItemData> rawData = DataService.CreateDataFromFilesAndMods<ItemData>("Items");

        foreach (ItemData data in rawData)
        {
            if (!data.Active) { continue; }

            ItemSO item = ScriptableObject.CreateInstance<ItemSO>();
            item.name = data.Id;
            item.Id = data.Id;
            item.Name = data.Name;
            item.Description = data.Description;
            item.BaseValue = data.BaseValue;
            item.MaxStackSize = data.MaxStackSize;
            item.ItemType = Util.ReturnEnumValueFromStringValue<ItemType>(data.ItemType);
            item.Rarity = Util.ReturnEnumValueFromStringValue<RarityType>(data.Rarity);
            item.FoodValue = data.FoodValue;


            scriptableObjects.Add(item);
        }

        base.CreateScriptableObjects();
    }

    public List<Item> GetItemsByType(ItemType itemType, List<RarityType> possibleRarities, int uniqueItems)
    {
        List<Item> items = new List<Item>();

        // get random unique items from count
        List<ItemSO> availableItems = Util.GetRandomValues(GetItemsByTypeAndRarity(itemType, possibleRarities), uniqueItems);

        foreach (ItemSO itemSO in availableItems)
        {
            int minCount = Mathf.Max(1, itemSO.MaxStackSize / 2);
            int itemCount = Util.GetRandomValue(minCount, itemSO.MaxStackSize);

            Item newItem = new Item();
            newItem.ItemBaseData = itemSO;
            newItem.Count = itemCount;
            items.Add(newItem);
        }

        return items;
    }

    private List<ItemSO> GetItemsByTypeAndRarity(ItemType itemType, List<RarityType> possibleRarities)
    {
        return scriptableObjects.FindAll(item => item.ItemType.Equals(itemType) && possibleRarities.Contains(item.Rarity));
    }
}
