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

    [SerializeField] private Color defaultOverlayColor;
    [SerializeField] private Color hoverOverlayColor;
    [SerializeField] private Color defaultIconColor;
    [SerializeField] private Color hoverIconColor;
    [SerializeField] private Color defaultTextColor;
    [SerializeField] private Color hoverTextColor;
    [Header("References")]
    [SerializeField] private Image overlayImage;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI label;

    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        OnPointerExit(null);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        RenderHover();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        RenderDefault();
    }

    private void RenderDefault()
    {
        rectTransform.DOLocalMoveY(defaultYPosition, tweenDuration).SetUpdate(true);
        overlayImage.CrossFadeColor(defaultOverlayColor, tweenDuration, true, true);
        icon.CrossFadeColor(defaultIconColor, tweenDuration, true, true);
        label.CrossFadeColor(defaultTextColor, tweenDuration, true, true);
    }

    private void RenderHover()
    {
        rectTransform.DOLocalMoveY(hoverYPosition, tweenDuration).SetUpdate(true);
        overlayImage.CrossFadeColor(hoverOverlayColor, tweenDuration, true, true);
        icon.CrossFadeColor(hoverIconColor, tweenDuration, true, true);
        label.CrossFadeColor(hoverTextColor, tweenDuration, true, true);
    }
}
