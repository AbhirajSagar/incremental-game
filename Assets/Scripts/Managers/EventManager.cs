public static class EventManager
{
    public static void Initialize()
    {
        // Bind core game events
        GameManager.Instance.Session.OnOxygenDepleted += HandleOxygenDepleted;
    }

    private static void HandleOxygenDepleted()
    {
        GameManager.Instance.EndGame();
        
        // Despawn fishes safely
        for (int i = Fish.AllFishes.Count - 1; i >= 0; i--)
        {
            Fish.AllFishes[i].OxygenFinishedDespawn();
        }

        UIManager.Instance.ShowDiveCompleteScreen();
    }
}