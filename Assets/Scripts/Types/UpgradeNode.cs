using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeNode : MonoBehaviour
{
    public ClickerButton UpgradeIconButton;
    public Image Icon;
    public RectTransform TooltipContainer;

    [Header("REFERENCES")]
    public TextMeshProUGUI UpgradeTitle;
    public TextMeshProUGUI UpgradePrice;
    public TextMeshProUGUI UpgradeDescription;
    public TextMeshProUGUI UpgradePropertyName;
    public TextMeshProUGUI UpgradePropertyChange;


    [Header("ANIMATION SETTINGS")]
    public float TooltipPopupDuration;
    public float IconShakeStrength;


    private Vector3 _OriginalIconScale;

    private void Awake()
    {
        UpgradeIconButton.onClick.RemoveAllListeners();
        UpgradeIconButton.OnPointerEnterAction += OnHoverStart;
        UpgradeIconButton.OnPointerExitAction += OnHoverEnd;
        _OriginalIconScale = Icon.transform.localScale;
    }

    public void Initialize(UpgradeData upgradeData)
    {
        
    }

    private void OnHoverEnd()
    {
        TooltipContainer.DOComplete();
        Icon.DOComplete();
        
        TooltipContainer.transform.localScale = Vector3.one;
        TooltipContainer.DOScale(Vector3.zero, TooltipPopupDuration).SetEase(Ease.InBack).SetLink(TooltipContainer.gameObject);
        Icon.transform.DOScale(_OriginalIconScale, 0.1f).SetLink(Icon.gameObject);
    }

    private void OnHoverStart()
    {
        TooltipContainer.DOComplete();
        Icon.DOComplete();

        TooltipContainer.transform.localScale = Vector3.zero;
        TooltipContainer.DOScale(Vector3.one, TooltipPopupDuration).SetEase(Ease.InBack).SetLink(TooltipContainer.gameObject);
        Icon.transform.DOScale(_OriginalIconScale * 0.8f, 0.1f).SetLink(Icon.gameObject);
    }
}