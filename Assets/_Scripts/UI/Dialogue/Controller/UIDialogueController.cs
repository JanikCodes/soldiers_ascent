using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class UIDialogueController : MonoBehaviour
{
    [Header("Configurations")]
    [SerializeField] private float defaultXPosition;
    [SerializeField] private float activeXPosition;
    [SerializeField] private float tweenDuration;

    [Header("References")]
    [SerializeField] private Transform choiceParent;

    [Header("Prefabs")]
    [SerializeField] private GameObject choicePrefab;

    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        PlayerDialogueHandler.OnDialogueInstantiated += HandleDialogueInstantiated;
    }

    private void OnDisable()
    {
        PlayerDialogueHandler.OnDialogueInstantiated -= HandleDialogueInstantiated;
    }

    private void HandleDialogueInstantiated(Transform self, Transform other)
    {
        DialogueTrigger dialogueTrigger = other.GetComponent<DialogueTrigger>();
        if (!dialogueTrigger)
        {
            Debug.LogWarning("Couldn't instantiate dialogue UI because DialogueTrigger is missing on the partner.");
            return;
        }

        // clear previous choices
        Util.ClearChildren(choiceParent);

        DialogueSO dialogueSO = dialogueTrigger.Dialogue;
        foreach (DialogueChoiceSO dialogueChoiceSO in dialogueSO.GetChoices(self, other))
        {
            GameObject choice = Instantiate(choicePrefab, choiceParent);
            UIDialogueChoiceItemController choiceController = choice.GetComponent<UIDialogueChoiceItemController>();
            choiceController.Setup(dialogueChoiceSO);
        }

        CallDialogueWindow();
    }

    private void CallDialogueWindow()
    {
        rectTransform.DOLocalMoveX(activeXPosition, tweenDuration).SetUpdate(true);
    }

    private void DismissDialogueWindow()
    {
        rectTransform.DOLocalMoveX(defaultXPosition, tweenDuration).SetUpdate(true);
    }
}
