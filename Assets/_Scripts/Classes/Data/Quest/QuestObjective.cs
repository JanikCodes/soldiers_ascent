using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/// <summary>
/// Class that is used to generate the ScriptableObject <see cref="QuestObjectiveSO"/> at runtime from.
/// </summary>
[Serializable]
public class QuestObjectiveData
{
    public string Text;
    public string Type;
    public Dictionary<string, object> Properties;
}

[Serializable]
public class QuestObjective
{
    public QuestObjectiveSO QuestObjectiveBaseData;
    public bool VisitedTarget;
    public int AquireCurrencyRemaining;

    public QuestObjective(QuestObjectiveSO data)
    {
        QuestObjectiveBaseData = data;
    }

    public bool IsCompleted()
    {
        return QuestObjectiveBaseData.IsComplete(this);
    }

    public void UpdateObjective()
    {
        QuestObjectiveBaseData.UpdateObjective(this);
    }

    public void Initialize()
    {
        QuestObjectiveBaseData.Initialize(this);
    }
}