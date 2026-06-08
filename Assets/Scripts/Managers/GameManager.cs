using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("Settings")]
    public GameConfig Config; 
    
    public static GameSession Session { get; private set; }
    public static SaveData CurrentSaveData { get; private set; }
    
    private float oxygenTimer;
    private bool isGameActive;

    protected override void Awake()
    {
        base.Awake();
        
        Session = new GameSession();
        CurrentSaveData = SaveManager.LoadData();

        Session.OnMoneyChanged += CurrentSaveData.SetMoney;

        Session.Initialize(Config, CurrentSaveData);
        EventManager.Initialize();
        
        isGameActive = true;
    }

    public void Start()
    {
        UpgradeManager.Instance.Initialize();
    }

    private void Update()
    {
        if (!isGameActive || Session.CurrentOxygen <= 0) return;

        oxygenTimer += Time.deltaTime * Session.State.OxygenDrainPerSecond;
        if (oxygenTimer >= 1f)
        {
            int amount = Mathf.FloorToInt(oxygenTimer);
            oxygenTimer -= amount;
            Session.DeductOxygen(amount);
        }
    }

    public void EndGame()
    {
        isGameActive = false;
        SaveManager.SaveNewData(CurrentSaveData);
    }
}