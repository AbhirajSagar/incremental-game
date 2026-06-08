using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    [SerializeField] private int TotalMoney;
    [SerializeField] private List<string> UnlockedNodes = new();

    public bool HasNode(string id) => UnlockedNodes.Contains(id);
    
    public void AddNodeId(string Id)
    {
        if (HasNode(Id)) return;
        
        UnlockedNodes.Add(Id);
        SaveManager.SaveNewData(this);
    }

    public void SetMoney(int money)
    {
        TotalMoney = money;
    }

    public int CurTotalMoney => TotalMoney;
    public IReadOnlyList<string> CurUnlockedNodes => UnlockedNodes;
}