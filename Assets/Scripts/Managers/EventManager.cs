using System;

public class EventManager
{
    public static void Initialize()
    {
        UIManager.Instance.OxygenMeterSystem.OnOxygenFinished += DespawnFishes;
        UIManager.Instance.OxygenMeterSystem.OnOxygenFinished += ShowDiveCompleteUI;
    }

    private static void ShowDiveCompleteUI()
    {
        UIManager.Instance.ShowDiveCompleteScreen();
    }

    private static void DespawnFishes()
    {
        Fish.AllFishes.ForEach(F => F.OxygenFinishedDespawn());
    }
}