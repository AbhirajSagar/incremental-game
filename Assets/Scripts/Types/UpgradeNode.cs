using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeNode : MonoBehaviour
{
    public bool Interactable
    {
        get => UpgradeIconButton.interactable;
        set => UpgradeIconButton.interactable = value;
    }

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
    private bool PurchasedAlready(string Id) => GameManager.CurrentSaveData.HasNode(Id);
    private bool IsAffordable(UpgradeData upgradeData) => GameManager.Session.CurrentMoney >= upgradeData.Price;

    public void Initialize(UpgradeData upgradeData)
    {
        (UpgradeTitle.text, UpgradePrice.text, UpgradeDescription.text, UpgradePropertyName.text, UpgradePropertyChange.text, Icon.sprite) = upgradeData;
        
        UIManager.Instance.OnUpgradeScreenShow += () => Interactable = IsAffordable(upgradeData);
        UpgradeIconButton.onClick.AddListener(() => UnlockUpgrade(upgradeData));

        UpgradeIconButton.OnPointerEnterAction += OnHoverStart;
        UpgradeIconButton.OnPointerExitAction += OnHoverEnd;
        
        _OriginalIconScale = Icon.transform.localScale;
    }

    private void UnlockUpgrade(UpgradeData upgradeData)
    {
        if (PurchasedAlready(upgradeData.Id)) return;
        
        GameManager.Session.DeductMoney(upgradeData.Price);
        GameManager.CurrentSaveData.AddNodeId(upgradeData.Id);
        upgradeData.Apply(GameManager.Session.State);
        OnHoverEnd();
        Interactable = false;
    }

    private void OnHoverEnd()
    {
        if(!Interactable) return;
        TooltipContainer.DOComplete();
        Icon.DOComplete();
        
        TooltipContainer.DOScale(Vector3.zero, TooltipPopupDuration).SetEase(Ease.InBack).SetLink(TooltipContainer.gameObject);
        Icon.transform.DOScale(_OriginalIconScale, 0.1f).SetLink(Icon.gameObject);
    }

    private void OnHoverStart()
    {
        if(!Interactable) return;
        TooltipContainer.DOComplete();
        Icon.DOComplete();

        TooltipContainer.transform.localScale = Vector3.zero;
        TooltipContainer.DOScale(Vector3.one, TooltipPopupDuration).SetEase(Ease.InBack).SetLink(TooltipContainer.gameObject);
        Icon.transform.DOScale(_OriginalIconScale * 0.8f, 0.1f).SetLink(Icon.gameObject);
    }
}