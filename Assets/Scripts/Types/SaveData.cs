using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    [SerializeField] private int TotalMoney;
    [SerializeField] private List<string> UnlockedNodes = new();
    [SerializeField] private AudioProfile AudioSettings = new(1f,1f,0.3f);

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

    public void SetBackgroundMusicVolume(float volume)
    {
        AudioSettings.BackgroundMusicVolume = volume;
        SaveManager.SaveNewData(this);
    }


    public void SetSFXVolume(float volume)
    {
        AudioSettings.SFXVolume = volume;
        SaveManager.SaveNewData(this);
    }

    public void SetAmbienceVolume(float volume)
    {
        AudioSettings.AmbienceVolume = volume;
        SaveManager.SaveNewData(this);
    }

    public int CurTotalMoney => TotalMoney;
    public float CurBGMVolume => AudioSettings.BackgroundMusicVolume;
    public float CurSFXVolume => AudioSettings.SFXVolume;
    public float CurAmbienceVolume => AudioSettings.AmbienceVolume;
    public IReadOnlyList<string> CurUnlockedNodes => UnlockedNodes;
}