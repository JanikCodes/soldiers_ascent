using System.Collections;
using System.Collections.Generic;
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
                dialogueChoice.rawJumpToDialogueId = dialogueChoiceData.JumpToDialogueId;

                dialogue.Choices.Add(dialogueChoice);
            }

            scriptableObjects.Add(dialogue);
        }

        // assign DialogueChoice property 'JumpToDialogue' after all DialogueSO's have been created
        foreach (DialogueSO dialogue in scriptableObjects)
        {
            foreach (DialogueChoiceSO dialogueChoice in dialogue.Choices)
            {
                dialogueChoice.JumpToDialogue = GetScriptableObject(dialogueChoice.rawJumpToDialogueId);
            }
        }

        base.CreateScriptableObjects();
    }
}
