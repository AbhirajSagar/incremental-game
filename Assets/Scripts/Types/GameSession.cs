using System;
using UnityEngine;

/// <summary>
/// Holds the volatile state for the current active run. 
/// </summary>
public class GameSession
{
    public int CurrentMoney { get; private set; }
    public int CurrentOxygen { get; private set; }

    // THE RUNTIME STATE MUTATED BY UPGRADES
    public GameState State { get; private set; }

    public event Action<int> OnMoneyChanged;
    public event Action<int, float> OnOxygenChanged;
    public event Action OnOxygenDepleted;

    public void Initialize(GameConfig config, SaveData savedData)
    {
        State = config.InitialState.Clone();

        CurrentMoney = savedData.CurTotalMoney;

        foreach(string NodeId in savedData.CurUnlockedNodes)
        {
            UpgradeManager.ApplyNodeOnSession(NodeId, this);
        }

        CurrentOxygen = State.MaxOxygen;
    }

    public void AddMoney(int amount)
    {
        CurrentMoney += amount;
        OnMoneyChanged?.Invoke(CurrentMoney);
    }

    public void DeductOxygen(int amount)
    {
        if (CurrentOxygen <= 0) return;

        CurrentOxygen = Mathf.Max(0, CurrentOxygen - amount);
        OnOxygenChanged?.Invoke(CurrentOxygen, (float)CurrentOxygen / State.MaxOxygen);
        
        if (CurrentOxygen == 0)
        {
            OnOxygenDepleted?.Invoke();
        }
    }

    public void DeductMoney(int price)
    {
        CurrentMoney -= price;
        OnMoneyChanged?.Invoke(CurrentMoney);
    }
}