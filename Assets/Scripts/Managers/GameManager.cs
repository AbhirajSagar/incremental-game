using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("Settings")]
    public GameConfig Config; 
    
    public GameSession Session { get; private set; }
    private float oxygenTimer;
    private bool isGameActive;

    protected override void Awake()
    {
        base.Awake();
        
        Session = new GameSession();
        SaveData Data = SaveManager.LoadData();

        Session.OnMoneyChanged += Data.SetMoney;

        Session.Initialize(Config, Data);
        EventManager.Initialize();
        
        isGameActive = true;
    }

    private void Update()
    {
        if (!isGameActive || Session.CurrentOxygen <= 0) return;

        oxygenTimer += Time.deltaTime;
        if (oxygenTimer >= Config.UnitOxygenDeductionDuration)
        {
            oxygenTimer = 0;
            Session.DeductOxygen(1, Config.MaxOxygen);
        }
    }

    public void EndGame()
    {
        isGameActive = false;
    }
}