using System;
using UnityEngine;

public class ConfigManager: Singleton<ConfigManager>
{
    [Range(1,15)]
    public float AttackRadius = 1;
    public float Damage = 1;
    public int Oxygen = 100;
    public float UnitOxygenDeductionDuration = 1;
    public int Money = 5;

    public Action<ConfigManager> OnConfigChanged;

    public void FishSold(int sellingPrice)
    {
        Money += sellingPrice;
        OnConfigChanged?.Invoke(this);
    }

    private void Start()
    {
        OnConfigChanged?.Invoke(this);
    }
}