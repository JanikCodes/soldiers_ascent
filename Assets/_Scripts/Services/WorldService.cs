using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldService : MonoBehaviour
{
    private FactionService factionService;

    private void Start()
    {
        factionService = GetComponentInChildren<FactionService>();

        FileService.CreateDirectories();

        factionService.CreateScriptableObjects();
    }
}
