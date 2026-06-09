using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickerButton : Button
{
    private RectTransform Shadow;
    private RectTransform Graphics;

    [SerializeField] private float PressDuration = 0.05f;
    public event Action OnPointerEnterAction;
    public event Action OnPointerExitAction;

    protected override void Awake()
    {
        base.Awake();
        Shadow = transform.GetChild(0).GetComponent<RectTransform>();
        Graphics = transform.GetChild(1).GetComponent<RectTransform>();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (!IsInteractable()) return; // [FIXED] Disable animation & clicks if Interactable is false
        base.OnPointerDown(eventData);

        Graphics.DOKill();
        Graphics.DOAnchorPos(Shadow.anchoredPosition, PressDuration).SetUpdate(true).SetLink(gameObject);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        OnPointerEnterAction?.Invoke();
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        OnPointerExitAction?.Invoke();
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (!IsInteractable()) return; // [FIXED]
        base.OnPointerUp(eventData);

        Graphics.DOKill();
        Graphics.DOAnchorPos(Vector2.zero, PressDuration).SetUpdate(true).SetLink(gameObject);
    }
}