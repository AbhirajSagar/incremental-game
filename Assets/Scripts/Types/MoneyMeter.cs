using System;
using TMPro;

[Serializable]
public class MoneyMeter : IConfigInitializable
{
    public TextMeshProUGUI Label;
    private int CurrentMoney
    {
        set
        {
            _CurMoney = value;
            Label.text = $"${value}";
        }

        get => _CurMoney;
    }

    private int _CurMoney;

    public void Initialize(ConfigManager Config, bool IsUpdate = true)
    {
        CurrentMoney = Config.Money;
    }
}