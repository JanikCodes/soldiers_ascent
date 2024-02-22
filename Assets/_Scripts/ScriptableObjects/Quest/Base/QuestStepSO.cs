using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class QuestStepSO : ScriptableObject
{
    public List<QuestObjectiveSO> Objectives = new();
}
