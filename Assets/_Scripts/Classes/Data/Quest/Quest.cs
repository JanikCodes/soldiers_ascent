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
    public bool UnlockedFromStart;
    public bool AutoComplete;
}

[Serializable]
public class Quest
{
    public QuestSO QuestBaseData;
    public List<QuestStep> Steps = new();
    public bool Unlocked;

    public Quest(Transform self, QuestSO data)
    {
        QuestBaseData = data;
        Unlocked = data.UnlockedFromStart;

        foreach (QuestStepSO questStepSO in data.Steps)
        {
            QuestStep questStep = new(self, questStepSO);
            questStep.InitializeObjectives();

            Steps.Add(questStep);
        }
    }

    public void UpdateSteps()
    {
        if (!Unlocked) { return; }

        // cycle through steps
        foreach (QuestStep questStep in Steps)
        {
            // check if index is completed yet
            if (!questStep.IsCompleted())
            {
                questStep.UpdateObjectives();

                // stop updating other steps
                return;
            }
        }
    }

    public bool IsCompleted()
    {
        foreach (var objective in Steps)
        {
            if (!objective.IsCompleted())
            {
                return false;
            }
        }
        return true;
    }
}