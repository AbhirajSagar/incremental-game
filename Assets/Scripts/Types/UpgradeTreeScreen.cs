using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[Serializable]
public class UpgradeTreeScreen : IUiScreen, IConfigurable
{
    public GameObject Root;

    public void ApplyConfig(GameConfig config) { }

    public void Hide()
    {
        Root.SetActive(false);
    }

    public void Show()
    {
        Root.SetActive(true);
    }
}