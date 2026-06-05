using System;
using UnityEngine;

/// <summary>
/// Holds the volatile state for the current active run. 
/// Not a MonoBehaviour. Re-instantiated every time a game starts.
/// </summary>
public class GameSession
{
    public int CurrentMoney { get; private set; }
    public int CurrentOxygen { get; private set; }

    public event Action<int> OnMoneyChanged;
    public event Action<int, float> OnOxygenChanged;
    public event Action OnOxygenDepleted;

    public void Initialize(GameConfig config, SaveData savedData)
    {
        CurrentOxygen = config.MaxOxygen;
        CurrentMoney = savedData.CurTotalMoney;

        foreach(string NodeId in savedData.CurUnlockedNodes)
        {
            UpgradeManager.ApplyNodeOnSession(NodeId, this);
        }
    }

    public void AddMoney(int amount)
    {
        CurrentMoney += amount;
        OnMoneyChanged?.Invoke(CurrentMoney);
    }

    public void DeductOxygen(int amount, int maxOxygen)
    {
        if (CurrentOxygen <= 0) return;

        CurrentOxygen = Mathf.Max(0, CurrentOxygen - amount);
        OnOxygenChanged?.Invoke(CurrentOxygen, (float)CurrentOxygen / maxOxygen);
        
        if (CurrentOxygen == 0)
        {
            OnOxygenDepleted?.Invoke();
        }
    }
}