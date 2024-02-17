using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerService : ScriptableObjectService<PlayerSO>
{
    [Header("Prefabs")]
    [SerializeField] private GameObject playerPrefab;

    private FactionService factionService;

    private void Awake()
    {
        factionService = transform.parent.GetComponentInChildren<FactionService>();
    }

    public override void CreateScriptableObjects()
    {
        List<PlayerData> rawData = DataService.CreateDataFromFilesAndMods<PlayerData>("Player");

        foreach (PlayerData data in rawData)
        {
            if (!data.Active) { continue; }

            PlayerSO player = ScriptableObject.CreateInstance<PlayerSO>();
            player.name = data.Id;
            player.Id = data.Id;
            player.InitiallyOwnedByFaction = data.InitiallyOwnedByFaction;
            player.SpawnPosition = new Vector3(data.SpawnPosition[0], data.SpawnPosition[1], data.SpawnPosition[2]);

            scriptableObjects.Add(player);
        }

        base.CreateScriptableObjects();
    }

    public void SpawnPlayerIntoWorld()
    {
        PlayerSO playerData = scriptableObjects[0];
        GameObject playerStructure = factionService.CreateAndSpawnArmy(playerData.SpawnPosition, playerData.InitiallyOwnedByFaction, playerPrefab);
    }
}
