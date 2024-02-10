using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldService : MonoBehaviour
{
    private SoldierService soldierService;
    private FactionService factionService;
    private StructureService structureService;

    private void Start()
    {
        soldierService = GetComponentInChildren<SoldierService>();
        factionService = GetComponentInChildren<FactionService>();
        structureService = GetComponentInChildren<StructureService>();

        // created neccesary file directories on startup
        FileService.CreateDirectories();

        soldierService.CreateScriptableObjects();

        factionService.CreateScriptableObjects();
        factionService.CreateFactionObjects();

        structureService.CreateScriptableObjects();
        structureService.CreateStructureObjects();
    }
}
