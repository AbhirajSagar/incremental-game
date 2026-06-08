public static class EventManager
{
    public static void Initialize()
    {
        GameManager.Session.OnOxygenDepleted -= HandleOxygenDepleted;
        GameManager.Session.OnOxygenDepleted += HandleOxygenDepleted;
    }

    private static void HandleOxygenDepleted()
    {
        GameManager.Instance.EndGame();
        FishSpawnManager.Instance.StopSpawning();
        UIManager.Instance.CustomCursor.Enabled = true;

        for (int i = Fish.AllFishes.Count - 1; i >= 0; i--)
        {
            Fish.AllFishes[i].OxygenFinishedDespawn();
        }

        UIManager.Instance.ShowDiveCompleteScreen();
    }
}