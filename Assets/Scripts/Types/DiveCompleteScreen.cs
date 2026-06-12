using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

[Serializable]
public class DiveCompleteScreen : IUiScreen, IConfigurable
{
    public GameObject Root;
    public Button ContinueButton;
    public Button UpgradeButton;

    [Header("LABELS")]
    public TextMeshProUGUI EarnedLabel;
    public TextMeshProUGUI MissedLabel;
    public TextMeshProUGUI AffordableUpgradesCountLabel;
    public GameObject AffordableUpgradesCountChip;
    public TextMeshProUGUI NextFishUnlockProgressLabel;
    public TextMeshProUGUI NextFishUnlockNameLabel;

    public void ApplyConfig(GameConfig config)
    {
        ContinueButton.onClick.RemoveAllListeners();
        UpgradeButton.onClick.RemoveAllListeners();

        ContinueButton.onClick.AddListener(Reload);
        UpgradeButton.onClick.AddListener(UIManager.Instance.ShowUpgradeScreen);
    }

    private void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Hide()
    {
        Root.transform.DOKill();
        Root.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack).OnComplete(() => Root.SetActive(false));
    }

    public void Show()
    {
        UpdateUI();
        Root.SetActive(true);
        Root.transform.DOKill();
        Root.transform.localScale = Vector3.zero;
        Root.transform.DOScale(Vector3.one, 0.4f).SetEase(Ease.OutBack);
    }

    private void UpdateUI()
    {
        EarnedLabel.text = $"${GameManager.Instance.MoneyEarned}";
        MissedLabel.text = $"{InputManager.Instance.MissedCount}";

        int Count = UpgradeManager.Instance.GetAffordableUpgradesCount();
        AffordableUpgradesCountLabel.text = $"{Count}";
        AffordableUpgradesCountChip.SetActive(Count > 0);
    }
}