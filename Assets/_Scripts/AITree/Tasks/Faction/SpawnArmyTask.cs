using System;
using System.Collections.Generic;
using System.Linq;
using RenownedGames.AITree;
using RenownedGames.Apex;
using UnityEngine;

[NodeContent("Spawn Army", "Tasks/Base/Faction/Spawn Army", IconPath = "Images/Icons/Node/SendMessageIcon.png")]
public class SpawnArmyTask : TaskNode
{
    [Header("Variables")]
    [SerializeField]
    private Vector3Key spawnLocation;
    [SerializeField]
    private IntKey squadAmount;

    // Stored required components.
    private ObjectStorage objectStorage;
    private FactionServiceReference factionServiceReference;
    private ItemService itemService;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        objectStorage = GetOwner().GetComponent<ObjectStorage>();
        factionServiceReference = GetOwner().GetComponent<FactionServiceReference>();
    }

    protected override void OnEntry()
    {
        base.OnEntry();
    }

    protected override State OnUpdate()
    {
        itemService = factionServiceReference.Service.GetOtherService<ItemService>();

        FactionSO factionData = objectStorage.GetObject<FactionSO>();
        FactionService factionService = factionServiceReference.Service;
        if (squadAmount.GetValue() == 0 || !factionService)
        {
            return State.Failure;
        }

        // spawn army
        GameObject armyRoot = factionService.CreateAndSpawnArmy(spawnLocation.GetValue(), factionData.Id);
        SquadStorage squadStorage = armyRoot.GetComponent<SquadStorage>();
        Inventory inventory = armyRoot.GetComponent<Inventory>();

        // select random squadPresets
        for (int i = 0; i < squadAmount.GetValue(); i++)
        {
            FactionSquadPresetSO preset = Util.GetRandomValue<FactionSquadPresetSO>(factionData.SquadPresets);

            Squad squad = new();
            foreach (SoldierSO soldierData in preset.Soldiers)
            {
                // add soldier to squad
                Soldier soldier = new(soldierData);
                squad.AddSoldier(soldier);
            }

            // add squad to storage
            squadStorage.AddSquad(squad);
        }

        // fill inventory based on present soldiers
        int soldierCount = squadStorage.GetTotalSoldierCount();

        int foodItemCount = soldierCount / 3;
        int resourceItemCount = soldierCount / 3;
        int luxuryItemCount = 0;

        // calculate luxury chance
        int luxuryChance = Util.GetRandomValue(0, 100);
        if (luxuryChance <= 25)
        {
            luxuryItemCount = Util.GetRandomValue(1, 2);
        }

        // TODO: calculate available item rarities based on army strength/size
        // TODO: perhaps move logic into seperate methodes across services/components to have them reuseable 
        List<RarityType> availableRarities = new();
        availableRarities.Add(RarityType.Common);

        List<Item> totalItems = new();
        totalItems.AddRange(itemService.GetItemsByType(ItemType.Food, availableRarities, foodItemCount));
        totalItems.AddRange(itemService.GetItemsByType(ItemType.Resource, availableRarities, resourceItemCount));
        totalItems.AddRange(itemService.GetItemsByType(ItemType.Luxury, availableRarities, luxuryItemCount));

        // add all items to the inventory
        foreach (Item item in totalItems)
        {
            inventory.AddItem(item);
        }

        return State.Success;
    }

    protected override void OnExit()
    {
        base.OnExit();
    }
}