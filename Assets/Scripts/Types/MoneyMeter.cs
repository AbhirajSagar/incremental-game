using System;
using TMPro;

[Serializable]
public class MoneyMeter : ISessionBindable
{
    public TextMeshProUGUI Label;

    public void Bind(GameSession session)
    {
        session.OnMoneyChanged += OnMoneyChanged;
    }

    public void Unbind(GameSession session)
    {
        session.OnMoneyChanged -= OnMoneyChanged;
    }

    public void ForceUpdate(int currentMoney)
    {
        OnMoneyChanged(currentMoney);
    }

    private void OnMoneyChanged(int currentMoney)
    {
        Label.text = $"${currentMoney}";
    }
}