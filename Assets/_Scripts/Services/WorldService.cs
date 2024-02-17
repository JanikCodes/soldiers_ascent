using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldService : MonoBehaviour
{
    private ItemService itemService;
    private RelationshipService relationshipService;
    private SoldierService soldierService;
    private FactionSquadPresetService factionSquadPresetService;
    private FactionService factionService;
    private PlayerService playerService;
    private StructureService structureService;

    private void Start()
    {
        itemService = GetComponentInChildren<ItemService>();
        relationshipService = GetComponentInChildren<RelationshipService>();
        soldierService = GetComponentInChildren<SoldierService>();
        factionSquadPresetService = GetComponentInChildren<FactionSquadPresetService>();
        factionService = GetComponentInChildren<FactionService>();
        playerService = GetComponentInChildren<PlayerService>();
        structureService = GetComponentInChildren<StructureService>();

        // created neccesary file directories on startup
        FileService.CreateDirectories();

        itemService.CreateScriptableObjects();

        relationshipService.CreateScriptableObjects();
        soldierService.CreateScriptableObjects();
        factionSquadPresetService.CreateScriptableObjects();

        factionService.CreateScriptableObjects();
        factionService.CreateFactionObjects();

        playerService.CreateScriptableObjects();
        playerService.SpawnPlayerIntoWorld();

        structureService.CreateScriptableObjects();
        structureService.CreateStructureObjects();

    }
}
