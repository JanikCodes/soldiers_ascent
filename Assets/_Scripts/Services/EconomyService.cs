using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This service will keep track of all item prices and will also update those prices over time. Other components can use <see cref="EconomyServiceReference"/> to check prices while trading or whatever.
/// </summary>
public class EconomyService : MonoBehaviour
{
    [Serializable]
    public struct ItemRecord
    {
        public string ItemId;
        public int Price;
    }

    [SerializeField] private List<ItemRecord> itemRecords = new List<ItemRecord>();

    private ItemService itemService;

    private void Awake()
    {
        itemService = transform.parent.GetComponentInChildren<ItemService>();
    }

    public void InitEconomy()
    {
        List<ItemSO> items = itemService.GetAllScriptableObjects();

        // create record for each existing item in the game
        foreach(ItemSO item in items)
        {
            ItemRecord record = new ItemRecord();
            record.ItemId = item.Id;
            record.Price = item.BaseValue;
            itemRecords.Add(record);
        }
    }

    public int GetPrice(string id)
    {
        return itemRecords.Find(item => item.ItemId.Equals(id)).Price;
    }
}
