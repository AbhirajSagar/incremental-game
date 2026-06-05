using System;
using DG.Tweening;
using UnityEngine;

[Serializable]
public class DiveCompleteScreen : IConfigInitializable, IUiScreen
{
    public GameObject Root;

    public void Hide()
    {
        Root.SetActive(false);
    }

    public void Initialize(ConfigManager Config, bool IsUpdate = true)
    {
        Root.SetActive(false);
    }

    public void Show()
    {
        Root.SetActive(true);
    }
}