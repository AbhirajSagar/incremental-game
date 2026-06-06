using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickerButton : Button
{
    private RectTransform Shadow;
    private RectTransform Graphics;

    [SerializeField] private float PressDuration = 0.05f;

    protected override void Awake()
    {
        base.Awake();

        Shadow = transform.GetChild(0).GetComponent<RectTransform>();
        Graphics = transform.GetChild(1).GetComponent<RectTransform>();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);

        Graphics.DOKill();
        Graphics.DOAnchorPos(Shadow.anchoredPosition, PressDuration).SetUpdate(true).SetLink(gameObject);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);

        Graphics.DOKill();
        Graphics.DOAnchorPos(Vector2.zero, PressDuration).SetUpdate(true).SetLink(gameObject);
    }
}