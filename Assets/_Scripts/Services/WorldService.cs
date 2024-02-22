using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldService : MonoBehaviour
{
    private DialogueService dialogueService;
    private QuestService questService;
    private ItemService itemService;
    private EconomyService economyService;
    private RelationshipService relationshipService;
    private SoldierService soldierService;
    private FactionSquadPresetService factionSquadPresetService;
    private FactionService factionService;
    private PlayerService playerService;
    private StructureService structureService;

    private void Start()
    {
        dialogueService = GetComponentInChildren<DialogueService>();
        questService = GetComponentInChildren<QuestService>();
        itemService = GetComponentInChildren<ItemService>();
        economyService = GetComponentInChildren<EconomyService>();
        relationshipService = GetComponentInChildren<RelationshipService>();
        soldierService = GetComponentInChildren<SoldierService>();
        factionSquadPresetService = GetComponentInChildren<FactionSquadPresetService>();
        factionService = GetComponentInChildren<FactionService>();
        playerService = GetComponentInChildren<PlayerService>();
        structureService = GetComponentInChildren<StructureService>();

        // created neccesary file directories on startup
        FileService.CreateDirectories();

        dialogueService.CreateScriptableObjects();
        questService.CreateScriptableObjects();

        itemService.CreateScriptableObjects();
        economyService.InitEconomy();

        relationshipService.CreateScriptableObjects();
        soldierService.CreateScriptableObjects();
        factionSquadPresetService.CreateScriptableObjects();

        factionService.CreateScriptableObjects();
        factionService.CreateFactionObjects();

        playerService.CreateScriptableObjects();
        playerService.SpawnPlayerIntoWorld();

        structureService.CreateScriptableObjects();
        structureService.CreateStructureObjects();

        if (SaveService.Instance)
        {
            // cache saveable objects
            SaveService.Instance.CacheAllSaveObjects();
            SaveService.Instance.CacheAllLoadObjects();
        }
        else
        {
            Debug.LogWarning("Loaded wrong scene! You're playing without the usage of the SaveService!");
        }

    }
}
