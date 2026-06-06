using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Settings/Game Config")]
public class GameConfig : ScriptableObject
{
    [Range(1, 15)] public float AttackRadius = 1;
    public float Damage = 1;
    public int MaxOxygen = 100;
    public float UnitOxygenDeductionDuration = 1;
}