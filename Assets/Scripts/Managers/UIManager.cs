using System;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public TargetAreaElement TargetAreaSystem;
    public OxygenMeter OxygenMeterSystem;
    public MoneyMeter MoneyMeterSystem;
    public DiveCompleteScreen DiveCompleteUI;
    public UpgradeTreeScreen UpgradeTreeUI;
    public CursorSystem CustomCursor;

    private void Start()
    {
        GameConfig config = GameManager.Instance.Config;
        GameSession session = GameManager.Instance.Session;

        // Apply Configurations
        TargetAreaSystem.ApplyConfig(config);
        OxygenMeterSystem.ApplyConfig(config);
        DiveCompleteUI.ApplyConfig(config);
        UpgradeTreeUI.ApplyConfig(config);

        // Bind logic to UI
        OxygenMeterSystem.Bind(session);
        MoneyMeterSystem.Bind(session);
        
        // Initial setup calls so UI starts with correct values
        MoneyMeterSystem.ForceUpdate(session.CurrentMoney);
        OxygenMeterSystem.ForceUpdate(session.CurrentOxygen, 1f);

        DiveCompleteUI.Hide();
        UpgradeTreeUI.Hide();
        CustomCursor.Enabled = false;
    }

    protected override void Subscribe()
    {
        InputManager.Instance.OnMouseMove += TargetAreaFollowMouse;
    }

    protected override void Unsubscribe()
    {
        if(InputManager.Instance != null)
        {
            InputManager.Instance.OnMouseMove -= TargetAreaFollowMouse;
        }
            
        if (GameManager.Instance != null && GameManager.Instance.Session != null)
        {
            OxygenMeterSystem.Unbind(GameManager.Instance.Session);
            MoneyMeterSystem.Unbind(GameManager.Instance.Session);
        }
    }

    private void TargetAreaFollowMouse(Vector2 vector)
    {
        TargetAreaSystem.Position = vector;
    }

    public void ShowDiveCompleteScreen()
    {
        DiveCompleteUI.Show();
    }

    public void ShowUpgradeScreen()
    {
        DiveCompleteUI.Hide();
        UpgradeTreeUI.Show();
    }

    public void ReturnToDiveCompleteScreen()
    {
        UpgradeTreeUI.Hide();
        DiveCompleteUI.Show();
    }
}