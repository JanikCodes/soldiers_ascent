using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private int inventoryMaxSize = 54;

    [SerializeField] private List<Item> items = new();

    public void ClearStorage()
    {
        items.Clear();
    }

    public List<Item> GetItems()
    {
        return items;
    }

    public void SetItems(List<Item> items)
    {
        this.items = items;
    }

    /// <summary>
    /// Automatically add a new item to the inventory, taking <see cref="Item.Count"/> and <see cref="Item.MaxStackSize"/> into account.
    /// It also finds the first empty slot and assigns it.
    /// </summary>
    /// <param name="item"></param>
    public void AddItem(Item item)
    {
        bool alreadyInInventory = false;
        int remainingAmount = item.Count;

        if (item.ItemBaseData.MaxStackSize.Equals(1))
        {
            Item newItem = (Item)item.Clone();
            newItem.SlotIndex = FindFirstEmptySlotIndex();
            items.Add(newItem);

            return;
        }

        foreach (Item itemData in items)
        {
            if (itemData.ItemBaseData.Equals(item.ItemBaseData))
            {

                if (itemData.Count + remainingAmount > itemData.ItemBaseData.MaxStackSize)
                {
                    int addingAmount = itemData.ItemBaseData.MaxStackSize - itemData.Count;
                    itemData.Count += addingAmount;
                    remainingAmount -= addingAmount;
                }
                else if (itemData.Count + remainingAmount <= itemData.ItemBaseData.MaxStackSize)
                {
                    itemData.Count += remainingAmount;
                    remainingAmount = 0;
                    alreadyInInventory = true;
                }
            }
        }

        if (!alreadyInInventory)
        {
            int cycleAmount = remainingAmount / item.ItemBaseData.MaxStackSize;

            for (int i = 0; i < cycleAmount; i++)
            {
                Item newItem = (Item)item.Clone();
                newItem.SlotIndex = FindFirstEmptySlotIndex();
                newItem.Count = item.ItemBaseData.MaxStackSize;
                items.Add(newItem);

                remainingAmount -= newItem.ItemBaseData.MaxStackSize;
            }

            if (remainingAmount > 0)
            {
                Item newItem = (Item)item.Clone();
                newItem.SlotIndex = FindFirstEmptySlotIndex();
                newItem.Count = remainingAmount;
                items.Add(newItem);
            }
        }
    }

    private int FindFirstEmptySlotIndex()
    {
        for (int i = 0; i < inventoryMaxSize; i++)
        {
            if (!items.Any(x => x.SlotIndex.Equals(i)))
            {
                return i;
            }
        }

        Debug.LogError("Couldn't find a valid empty slot index!");
        return -99;
    }
}
