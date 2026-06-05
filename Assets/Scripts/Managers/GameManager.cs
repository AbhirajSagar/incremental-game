public class GameManager : Singleton<GameManager>
{
    protected override void Awake()
    {
        base.Awake();
        EventManager.Initialize();
    }

    private void Start()
    {
        
    }
}