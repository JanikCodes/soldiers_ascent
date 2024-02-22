using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that is used to generate the ScriptableObject <see cref="QuestSO"/> at runtime from.
/// </summary>
[Serializable]
public class QuestData : BaseData
{
    public string Title;
    public string Description;
    public QuestStepData[] Steps;
    public bool AutoComplete;
}