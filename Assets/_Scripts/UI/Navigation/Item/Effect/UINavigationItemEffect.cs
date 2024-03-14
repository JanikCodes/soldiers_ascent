using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class UINavigationItemEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Configurations")]
    [SerializeField] private float defaultYPosition;
    [SerializeField] private float hoverYPosition;
    [SerializeField] private float tweenDuration;

    [SerializeField] private Color defaultColor;
    [SerializeField] private Color hoverColor;
    [Header("References")]
    [SerializeField] private Image overlayImage;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI label;

    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        rectTransform.DOLocalMoveY(hoverYPosition, tweenDuration).SetUpdate(true);
        overlayImage.CrossFadeColor(hoverColor, tweenDuration, true, true);
        icon.CrossFadeColor(hoverColor, tweenDuration, true, true);
        label.CrossFadeColor(hoverColor, tweenDuration, true, true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        rectTransform.DOLocalMoveY(defaultYPosition, tweenDuration).SetUpdate(true);
        overlayImage.CrossFadeColor(defaultColor, tweenDuration, true, true);
        icon.CrossFadeColor(defaultColor, tweenDuration, true, true);
        label.CrossFadeColor(defaultColor, tweenDuration, true, true);
    }
}
