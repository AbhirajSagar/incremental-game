using System;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public TargetAreaElement TargetAreaSystem;
    public OxygenMeter OxygenMeterSystem;

    private IConfigInitializable[] ConfigInitializables;

    private void Start()
    {
        ConfigInitializables = new IConfigInitializable[] 
        { 
            TargetAreaSystem,
            OxygenMeterSystem
        };

        UpdateConfig(ConfigManager.Instance);
    }

    protected override void Subscribe()
    {
        InputManager.Instance.OnMouseMove += TargetAreaFollowMouse;
        ConfigManager.Instance.OnConfigChanged += UpdateConfig;
    }

    private void UpdateConfig(ConfigManager Config)
    {
        Array.ForEach(ConfigInitializables, C => C.Initialize(Config));
    }

    protected override void Unsubscribe()
    {
        InputManager.Instance.OnMouseMove -= TargetAreaFollowMouse;   
    }

    private void TargetAreaFollowMouse(Vector2 vector)
    {
        TargetAreaSystem.Position = vector;
    }
}