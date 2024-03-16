using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIDialogueChoiceItemController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI label;

    private DialogueChoiceSO data;
    private Transform self, other;
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();

        button.onClick.AddListener(OnClick);
    }

    public void Setup(DialogueChoiceSO dialogueChoiceSO, Transform self, Transform other)
    {
        data = dialogueChoiceSO;

        label.color = dialogueChoiceSO.TextColor;
        label.text = data.ChoiceText;

        this.self = self;
        this.other = other;
    }

    /// <summary>
    /// Executes all actions attached to the dialogueAction
    /// </summary>
    public void OnClick()
    {
        data.ExecuteActions(self, other);
    }
}
