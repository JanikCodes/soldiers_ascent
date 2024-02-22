using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class DialogueService : ScriptableObjectService<DialogueSO>
{
    public override void CreateScriptableObjects()
    {
        List<DialogueData> rawDataDialogues = DataService.CreateDataFromFilesAndMods<DialogueData>("Dialogues");
        List<DialogueChoiceData> rawDataDialogueChoices = DataService.CreateDataFromFilesAndMods<DialogueChoiceData>("DialogueChoices");

        foreach (DialogueData dialogueData in rawDataDialogues)
        {
            if (!dialogueData.Active) { continue; }

            DialogueSO dialogue = ScriptableObject.CreateInstance<DialogueSO>();
            dialogue.name = dialogueData.Id;
            dialogue.Id = dialogueData.Id;

            // assign choices to dialogue
            foreach (DialogueChoiceData dialogueChoiceData in rawDataDialogueChoices)
            {
                if (!dialogueChoiceData.Active) { continue; }
                // ignore if that choice isnt assigned to this dialogue
                if (!dialogueChoiceData.AssignedDialogueId.Equals(dialogue.Id)) { continue; }

                DialogueChoiceSO dialogueChoice = ScriptableObject.CreateInstance<DialogueChoiceSO>();
                dialogueChoice.name = dialogueChoiceData.Id;
                dialogueChoice.Id = dialogueChoiceData.Id;
                dialogueChoice.ChoiceText = dialogueChoiceData.ChoiceText;
                dialogueChoice.RawJumpToDialogueId = dialogueChoiceData.JumpToDialogueId;
                dialogueChoice.Requirements = GenerateRequirements(dialogueChoiceData.Requirements);

                dialogue.Choices.Add(dialogueChoice);
            }

            scriptableObjects.Add(dialogue);
        }

        // assign DialogueChoice property 'JumpToDialogue' after all DialogueSO's have been created
        foreach (DialogueSO dialogue in scriptableObjects)
        {
            foreach (DialogueChoiceSO dialogueChoice in dialogue.Choices)
            {
                dialogueChoice.JumpToDialogue = GetScriptableObject(dialogueChoice.RawJumpToDialogueId);
            }
        }

        base.CreateScriptableObjects();
    }

    private List<DialogueRequirementSO> GenerateRequirements(DialogueRequirementData[] requirements)
    {
        List<DialogueRequirementSO> results = new();

        // catch early if requirements are null
        if (requirements == null) { return results; }

        // try adding each requirement scriptableobject
        foreach (DialogueRequirementData requirementData in requirements)
        {
            string type = requirementData.Type;
            Type requirementType = Type.GetType($"Dialogue{type}RequirementSO");

            if (requirementType == null || !typeof(DialogueRequirementSO).IsAssignableFrom(requirementType))
            {
                Debug.LogWarning($"Invalid condition type: {type}");
                continue;
            }

            // generate object based on requirementType
            DialogueRequirementSO requirementSO = ScriptableObject.CreateInstance(requirementType) as DialogueRequirementSO;

            if (requirementSO == null)
            {
                Debug.LogWarning($"Failed to create an instance of {type}");
                continue;
            }

            // try to populate the object with its properties
            foreach (KeyValuePair<string, object> property in requirementData.Properties)
            {
                if (!WriteValueToField(type, requirementType, requirementSO, property))
                {
                    continue;
                }
            }

            results.Add(requirementSO);
        }

        return results;
    }

    private bool WriteValueToField<T>(string type, Type requirementType, T requirement, KeyValuePair<string, object> property)
    {
        FieldInfo fieldInfo = requirementType.GetField(property.Key, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

        if (fieldInfo == null)
        {
            Debug.LogWarning($"Field {property.Key} not found in requirement {type}. Using the default value.");
            return false;
        }

        Type fieldInfoType = fieldInfo.FieldType;
        object value = property.Value;

        try
        {
            // Convert the value to the type of the field ( int64 -> int32 )
            value = Convert.ChangeType(value, fieldInfoType);
        }
        catch (InvalidCastException)
        {
            Debug.LogWarning($"Type mismatch for field {property.Key} in requirement {type}. Using the default value.");
            return false;
        }

        fieldInfo.SetValue(requirement, value);

        return true;
    }
}
