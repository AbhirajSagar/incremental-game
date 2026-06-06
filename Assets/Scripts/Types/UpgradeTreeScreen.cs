using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class UpgradeTreeScreen : IUiScreen, IConfigurable
{
    public GameObject Root;
    public Button BackButton;

    public void ApplyConfig(GameConfig config)
    {
        BackButton.onClick.RemoveAllListeners();
        BackButton.onClick.AddListener(UIManager.Instance.ReturnToDiveCompleteScreen);
    }

    private void InitializeTreeUI(GameConfig config)
    {
        
    }

    public void Hide()
    {
        Root.SetActive(false);
    }

    public void Show()
    {
        Root.SetActive(true);
    }
}