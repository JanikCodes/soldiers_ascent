using System.Collections.Generic;
using UnityEngine;

public abstract class ScriptableObjectService<T> : MonoBehaviour where T : DataSO
{
    [Header("ScriptableObjects")]
    [SerializeField] protected List<T> scriptableObjects = new();

    public virtual void CreateScriptableObjects()
    {
        Debug.Log("Type of: " + typeof(T).Name);
        Debug.Log("Number of scriptableObjects created: " + scriptableObjects.Count);
    }

    public List<T> GetAllScriptableObjects()
    {
        return scriptableObjects;
    }

    public T GetScriptableObject(string id)
    {
        return scriptableObjects.Find(x => x.Id.Equals(id));
    }

    public U GetOtherService<U>()
    {
        return transform.parent.GetComponentInChildren<U>();
    }
}