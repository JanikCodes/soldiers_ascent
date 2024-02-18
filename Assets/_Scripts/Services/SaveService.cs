using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RenownedGames.Apex;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveService : MonoBehaviour
{
    public static SaveService Instance { get; private set; }

    private List<ISave> saveObjects = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        SceneManager.LoadSceneAsync("WorldScene");
    }

    public void Save()
    {
        Save save = new();
        save.Version = 0.4f;

        foreach (ISave saveable in saveObjects)
        {
            saveable.Save(save);
        }

        FileService.SaveToFile(save);
    }

    public void CacheAllSaveObjects()
    {
        IEnumerable<ISave> objects = FindObjectsOfType<MonoBehaviour>().OfType<ISave>();
        saveObjects = new List<ISave>(objects); 
    }
}

[CustomEditor(typeof(SaveService))]
public class CustomInspectorButton : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SaveService myScript = (SaveService)target;

        if (GUILayout.Button("Save"))
        {
            myScript.Save();
        }
    }
}