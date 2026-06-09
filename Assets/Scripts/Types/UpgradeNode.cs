using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public enum UpgradeNodeState
{
    Inactive,
    Locked,
    Unlocked
}
[Serializable]
public class UpgradeNodeVisualState
{
    [Header("Interaction")]
    public bool Interactable;

    [Header("Button")]
    public Color ButtonColor = Color.white;
    public Color ShadowColor = Color.white;

    [Header("Icon")]
    public Color IconColor = Color.white;
    public bool ShowCheckmark;

    [Header("Outline")]
    public Color OutlineColor = Color.white;

    [Header("Text Visibility")]
    public bool ShowPrice = true;

    [Header("Container Colors")]
    public Color TooltipContainerBackground = Color.white;
    public Color TooltipHeaderBackground = Color.white;
    public Color TooltipShadowBackground = Color.white;

    [Header("Text Colors")]
    public Color TitleColor = Color.white;
    public Color PriceColor = Color.white;
    public Color DescriptionColor = Color.white;
    public Color PropertyNameColor = Color.white;
    public Color PropertyChangeColor = Color.white;
}

public class UpgradeNode : MonoBehaviour
{
    public bool Interactable
    {
        get => UpgradeIconButton.interactable;
        set => UpgradeIconButton.interactable = value;
    }

    [Header("References")]
    public ClickerButton UpgradeIconButton;
    public RectTransform TooltipContainer;
    public float DelayInNodeAnim = 0.05f;
    
    [Header("Components")]
    public Image TooltipContainerBackground;
    public Image TooltipHeaderBackground;
    public Image TooltipShortDescriptionBackground;
    public Image TooltipShadowBackground;

    public Image CheckIcon;
    public Image ButtonGraphics;
    public Image ShadowGraphics;
    public Outline ButtonOutline;
    public Image Icon;

    public TextMeshProUGUI UpgradeTitle;
    public TextMeshProUGUI UpgradePrice;
    public TextMeshProUGUI UpgradeDescription;
    public TextMeshProUGUI UpgradePropertyName;
    public TextMeshProUGUI UpgradePropertyChange;

    [Header("States")]
    public UpgradeNodeVisualState InactiveState;
    public UpgradeNodeVisualState LockedState;
    public UpgradeNodeVisualState UnlockedState;

    [Header("Animation")]
    public float TooltipPopupDuration = 0.2f;
    public float HoverScaleMultiplier = 0.8f;

#if UNITY_EDITOR
    [Header("Editor Preview")]
    public bool PreviewInEditor;
    public UpgradeNodeState PreviewState;
#endif

    private UpgradeData _upgradeData;
    private Vector3 _originalIconScale;

    private bool PurchasedAlready => GameManager.CurrentSaveData.HasNode(_upgradeData.Id);
    private bool Affordable => GameManager.Session.CurrentMoney >= _upgradeData.Price;
    private Vector3 OrgScale;


    private void AnimateInNode(float delay)
    {
        OrgScale = transform.localScale;
        transform.localScale = Vector3.zero;
        transform.DOScale(OrgScale, 0.2f).SetEase(Ease.OutBack).SetDelay(delay);
    }

    public void Initialize(UpgradeData upgradeData, int i, out bool purchased)
    {
        _upgradeData = upgradeData;

        (UpgradeTitle.text,
         UpgradePrice.text,
         UpgradeDescription.text,
         UpgradePropertyName.text,
         UpgradePropertyChange.text,
         Icon.sprite) = upgradeData;

        purchased = PurchasedAlready;

        RefreshState();

        UpgradeIconButton.onClick.AddListener(OnClicked);
        UpgradeIconButton.OnPointerEnterAction += OnHoverStart;
        UpgradeIconButton.OnPointerExitAction += OnHoverEnd;
        UIManager.Instance.OnUpgradeScreenShow += RefreshState;

        _originalIconScale = Icon.transform.localScale;
        AnimateInNode(i * DelayInNodeAnim);
    }

    private void RefreshState()
    {
        if (PurchasedAlready)
        {
            ApplyState(UpgradeNodeState.Unlocked);
            return;
        }

        ApplyState(Affordable ? UpgradeNodeState.Locked : UpgradeNodeState.Inactive);
    }

    public void ApplyState(UpgradeNodeState state)
    {
        UpgradeNodeVisualState visuals = state switch
        {
            UpgradeNodeState.Inactive => InactiveState,
            UpgradeNodeState.Locked => LockedState,
            UpgradeNodeState.Unlocked => UnlockedState,
            _ => InactiveState
        };

        Interactable = visuals.Interactable;

        ButtonGraphics.color = visuals.ButtonColor;
        ShadowGraphics.color = visuals.ShadowColor;

        Icon.color = visuals.IconColor;

        CheckIcon.enabled = visuals.ShowCheckmark;
        UpgradePrice.enabled = visuals.ShowPrice;

        ButtonOutline.effectColor = visuals.OutlineColor;

        TooltipContainerBackground.color = TooltipShortDescriptionBackground.color = visuals.TooltipContainerBackground;
        TooltipHeaderBackground.color = visuals.TooltipHeaderBackground;
        TooltipShadowBackground.color = visuals.TooltipShadowBackground;

        UpgradeTitle.color = visuals.TitleColor;
        UpgradePrice.color = visuals.PriceColor;
        UpgradeDescription.color = visuals.DescriptionColor;
        UpgradePropertyName.color = visuals.PropertyNameColor;
        UpgradePropertyChange.color = visuals.PropertyChangeColor;
    }

    private void OnClicked()
    {
        if (PurchasedAlready || !Affordable)
        {
            return;
        }

        GameManager.Session.DeductMoney(_upgradeData.Price);
        GameManager.CurrentSaveData.AddNodeId(_upgradeData.Id);

        _upgradeData.Apply(GameManager.Session.State);

        OnHoverEnd();
        RefreshState();
    }

    private void OnHoverStart()
    {
        TooltipContainer.DOComplete();
        Icon.transform.DOComplete();

        TooltipContainer.localScale = Vector3.zero;

        TooltipContainer
            .DOScale(Vector3.one, TooltipPopupDuration)
            .SetEase(Ease.OutBack)
            .SetLink(TooltipContainer.gameObject);

        Icon.transform
            .DOScale(_originalIconScale * HoverScaleMultiplier, 0.1f)
            .SetLink(Icon.gameObject);
    }

    private void OnHoverEnd()
    {
        TooltipContainer.DOComplete();
        Icon.transform.DOComplete();

        TooltipContainer
            .DOScale(Vector3.zero, TooltipPopupDuration)
            .SetEase(Ease.InBack)
            .SetLink(TooltipContainer.gameObject);

        Icon.transform
            .DOScale(_originalIconScale, 0.1f)
            .SetLink(Icon.gameObject);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (Application.isPlaying || !PreviewInEditor)
        {
            return;
        }

        ApplyState(PreviewState);
    }
#endif
}