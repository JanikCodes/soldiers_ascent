using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIDialogueChoiceItemController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI label;

    private DialogueChoiceSO data;

    public void Setup(DialogueChoiceSO dialogueChoiceSO)
    {
        data = dialogueChoiceSO;

        label.color = dialogueChoiceSO.TextColor;
        label.text = data.ChoiceText;
    }
}
