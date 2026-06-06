using System;
using UnityEngine;
using UnityEngine.UI;

public enum STAT_TYPE
{
    ATTACK_RADIUS_INCREASE,
    DAMAGE_INCREASE
}


[CreateAssetMenu(fileName = "UpgradeData", menuName = "Settings/New Upgrade")]
public class UpgradeData : ScriptableObject
{
    public string Id = "";
    public STAT_TYPE statType;
    public int Value;
    public int Price;
    public Sprite Icon;
    public string Description;
    

    [ContextMenu("Generate Id")]
    public void GenerateId()
    {
        Id = Guid.NewGuid().ToString("N");

    }
}