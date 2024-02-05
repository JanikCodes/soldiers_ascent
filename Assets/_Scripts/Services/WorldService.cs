using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldService : MonoBehaviour
{
    private FactionService factionService;
    private StructureService structureService;

    private void Start()
    {
        factionService = GetComponentInChildren<FactionService>();
        structureService = GetComponentInChildren<StructureService>();

        FileService.CreateDirectories();

        factionService.CreateScriptableObjects();
        structureService.CreateScriptableObjects();
    }
}
