using System.Collections.Generic;
using UnityEngine;

public abstract class ScriptableObjectService<T> : MonoBehaviour where T : DataSO
{   
    [Header("ScriptableObjects")]
    [SerializeField] protected List<T> scriptableObjects = new List<T>();

    public virtual void CreateScriptableObjects()
    {
        Debug.Log("Type of : " + typeof(T).Name);
        Debug.Log("Number of scriptableObjects created: " + scriptableObjects.Count);
    }

    public List<T> GetAllSO()
    {
        return scriptableObjects;
    }

    public T GetSOById(string id)
    {
        return scriptableObjects.Find(x => x.Id.Equals(id));
    }
}