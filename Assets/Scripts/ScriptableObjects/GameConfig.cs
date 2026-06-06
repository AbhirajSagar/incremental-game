using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Settings/Game Config")]
public class GameConfig : ScriptableObject
{
    [Header("ATTACK")]
    [Range(1, 15)] 
    public float AttackRadius = 1;

    [Header("OXYGEN")]
    public int MaxOxygen = 100;
    public float UnitOxygenDeductionDuration = 1f;
    public float OxygenGreaterThanDamageMultiplier = 1f;
    public float OxygenGreaterThanPercent = 90f;

    [Header("CRITICAL DAMAGE")]
    public float CriticalDamageChance = 0f;`
    public float CriticalDamageMultiplier = 1f;
    public float OxygenRestorePerKill = 0f;

    [Header("DAMAGE")]
    public float Damage = 1;

    [Header("FULL HEALTH FISHES")]
    public float ExtraDamageChanceOnFullHealthFishes = 0f;
    public float ExtraDamageMultiplierOnFullHealthFishes = 1f;

    [Header("GOLDEN FISH")]
    public float GoldenFishSpawnChance = 0f;

    [Header("JACKPOT FISH")]
    public float JackpotFishChance = 0f;
    public float JackpotFishPriceMultiplier = 1f;
}