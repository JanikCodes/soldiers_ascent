using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that is used to generate the ScriptableObject <see cref="QuestStepSO"/> at runtime from.
/// </summary>
[Serializable]
public class QuestStepData
{
    public QuestObjectiveData[] Objectives;
}