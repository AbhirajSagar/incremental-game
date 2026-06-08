using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class OxygenMeter : IConfigurable, ISessionBindable
{
    public Image FillAmount;
    public RectTransform FillSlider;
    public TextMeshProUGUI Label;

    public float SlideWidthAdjust = 2.56f;
    private int MaxOxygen;

    public void ApplyConfig(GameConfig config)
    {
        FillSlider.sizeDelta = new Vector2(GameManager.Session.State.MaxOxygen * SlideWidthAdjust, FillSlider.sizeDelta.y);
        MaxOxygen = GameManager.Session.State.MaxOxygen;
    }

    public void Bind(GameSession session)
    {
        session.OnOxygenChanged += OnOxygenChanged;
    }

    public void Unbind(GameSession session)
    {
        session.OnOxygenChanged -= OnOxygenChanged;
    }

    public void ForceUpdate(int currentOxygen, float normalized)
    {
        OnOxygenChanged(currentOxygen, normalized);
    }

    private void OnOxygenChanged(int currentOxygen, float normalizedValue)
    {
        FillAmount.fillAmount = normalizedValue;
        Label.text = $"{currentOxygen}/{MaxOxygen}";
    }
}