using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[Serializable]
public class DiveCompleteScreen : IUiScreen, IConfigurable
{
    public GameObject Root;
    public Button ContinueButton;

    public void ApplyConfig(GameConfig config)
    {
        ContinueButton.onClick.RemoveAllListeners();
        ContinueButton.onClick.AddListener(Reload);
    }

    private void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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