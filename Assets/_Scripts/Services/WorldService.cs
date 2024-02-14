using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldService : MonoBehaviour
{
    // TODO: replace with proper service
    [SerializeField] private GameObject playerPrefab;

    private SoldierService soldierService;
    private RelationshipService relationshipService;
    private FactionSquadPresetService factionSquadPresetService;
    private FactionService factionService;
    private StructureService structureService;

    private void Start()
    {
        relationshipService = GetComponentInChildren<RelationshipService>();
        soldierService = GetComponentInChildren<SoldierService>();
        factionSquadPresetService = GetComponentInChildren<FactionSquadPresetService>();
        factionService = GetComponentInChildren<FactionService>();
        structureService = GetComponentInChildren<StructureService>();

        // created neccesary file directories on startup
        FileService.CreateDirectories();

        relationshipService.CreateScriptableObjects();
        soldierService.CreateScriptableObjects();
        factionSquadPresetService.CreateScriptableObjects();

        factionService.CreateScriptableObjects();
        factionService.CreateFactionObjects();

        structureService.CreateScriptableObjects();
        structureService.CreateStructureObjects();

        // TODO: remove temporary player creation here
        Instantiate(playerPrefab);
    }
}
