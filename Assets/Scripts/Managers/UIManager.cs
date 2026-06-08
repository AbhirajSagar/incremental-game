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


    public event Action OnUpgradeScreenShow;

    private void Start()
    {
        GameConfig config = GameManager.Instance.Config;
        GameSession session = GameManager.Session;

        TargetAreaSystem.ApplyConfig(config);
        OxygenMeterSystem.ApplyConfig(config);
        DiveCompleteUI.ApplyConfig(config);
        UpgradeTreeUI.ApplyConfig(config);

        OxygenMeterSystem.Bind(session);
        MoneyMeterSystem.Bind(session);
        
        // [FIXED] Oxygen shouldn't automatically assume 1f normal value if initialized with modified starting O2
        MoneyMeterSystem.ForceUpdate(session.CurrentMoney);
        float normalizedO2 = session.State.MaxOxygen > 0 ? (float)session.CurrentOxygen / session.State.MaxOxygen : 0;
        OxygenMeterSystem.ForceUpdate(session.CurrentOxygen, normalizedO2);

        DiveCompleteUI.Hide();
        UpgradeTreeUI.Hide();
        CustomCursor.Enabled = false;
    }

    protected override void Subscribe()
    {
        if (InputManager.Instance != null)
            InputManager.Instance.OnMouseMove += TargetAreaFollowMouse;
    }

    protected override void Unsubscribe()
    {
        if (InputManager.Instance != null)
            InputManager.Instance.OnMouseMove -= TargetAreaFollowMouse;
            
        if (GameManager.Session != null)
        {
            OxygenMeterSystem.Unbind(GameManager.Session);
            MoneyMeterSystem.Unbind(GameManager.Session);
        }
        
        // [FIXED] Failing to disable CustomCursor here leaked an event subscription in CursorSystem across scenes
        CustomCursor.Enabled = false;
    }

    private void TargetAreaFollowMouse(Vector2 vector) => TargetAreaSystem.Position = vector;

    public void ShowDiveCompleteScreen() => DiveCompleteUI.Show();
    
    public void ShowUpgradeScreen()
    {
        DiveCompleteUI.Hide();
        UpgradeTreeUI.Show();
        OnUpgradeScreenShow?.Invoke();
    }

    public void ReturnToDiveCompleteScreen()
    {
        UpgradeTreeUI.Hide();
        DiveCompleteUI.Show();
    }
}