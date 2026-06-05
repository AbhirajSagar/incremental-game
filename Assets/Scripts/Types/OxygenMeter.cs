using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class OxygenMeter : IConfigInitializable
{
    public Image FillAmount;
    public RectTransform FillSlider;
    public TextMeshProUGUI Label;

    public float SlideWidthAdjust = 2.56f;
    private int CurrentOxygen
    {
        get => _CurrentOxygen;
        set
        {
            _CurrentOxygen = value;
            FillAmount.fillAmount = (float) CurrentOxygen / MaxOxygen;    
        }
    }

    private int _CurrentOxygen;
    private int MaxOxygen = -1;
    private bool CanDecreaseOxygen = false;
    private Tween OxygenDeductTween;
    private float UnitOxygenDeductionDuration = 1f;

    public event Action OnOxygenFinished;

    public void Initialize(ConfigManager Config, bool IsUpdate = true)
    {
        MaxOxygen = CurrentOxygen = Config.Oxygen;
        UnitOxygenDeductionDuration = Config.UnitOxygenDeductionDuration;
        FillAmount.fillAmount = 1;

        FillSlider.sizeDelta = new Vector2(Config.Oxygen * SlideWidthAdjust, FillSlider.sizeDelta.y);

        CanDecreaseOxygen = true;
        
        OxygenDeductTween?.Kill();
        OxygenDeductTween = null;
        OxygenDeductTween = DOVirtual.DelayedCall(UnitOxygenDeductionDuration, () => DeductOxygen()).SetLoops(-1, LoopType.Restart);
    }

    private void DeductOxygen()
    {
        CurrentOxygen = Math.Max(0, CurrentOxygen - 1);
        if(CurrentOxygen != 0) return;
        Label.text = $"{CurrentOxygen/MaxOxygen}";

        OnOxygenFinished?.Invoke();
    }
}