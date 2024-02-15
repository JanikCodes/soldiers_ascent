using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldService : MonoBehaviour
{
    private SoldierService soldierService;
    private RelationshipService relationshipService;
    private FactionSquadPresetService factionSquadPresetService;
    private FactionService factionService;
    private StructureService structureService;
    private PlayerService playerService;

    private void Start()
    {
        relationshipService = GetComponentInChildren<RelationshipService>();
        soldierService = GetComponentInChildren<SoldierService>();
        factionSquadPresetService = GetComponentInChildren<FactionSquadPresetService>();
        factionService = GetComponentInChildren<FactionService>();
        playerService = GetComponentInChildren<PlayerService>();
        structureService = GetComponentInChildren<StructureService>();

        // created neccesary file directories on startup
        FileService.CreateDirectories();

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
