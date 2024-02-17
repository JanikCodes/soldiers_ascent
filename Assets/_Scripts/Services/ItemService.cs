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
            item.PossibleRarities = Util.ReturnEnumValuesFromStringValues<RarityType>(data.PossibleRarities);
            item.FoodValue = data.FoodValue;


            scriptableObjects.Add(item);
        }

        base.CreateScriptableObjects();
    }
}
