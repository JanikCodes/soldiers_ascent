using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestStorage : MonoBehaviour
{
    [field: SerializeField] public List<Quest> Quests { get; private set; }

    private void Awake()
    {
        Quests = new();
    }

    /// <summary>
    /// Converts all available quests from <see cref="QuestService"/> to real <see cref="Quest"/> objects.
    /// </summary>
    /// <param name="data"></param>
    public void InstantiateQuests(List<QuestSO> data)
    {
        foreach (QuestSO questSO in data)
        {
            Quest quest = new Quest(questSO);
            Quests.Add(quest);
        }
    }

    private void Update()
    {
        UpdateQuests();
    }

    /// <summary>
    /// Handles the updating for quest objectives based on if a quest step is not completed.
    /// </summary>
    private void UpdateQuests()
    {
        foreach (Quest quest in Quests)
        {
            quest.UpdateSteps();
        }
    }
}