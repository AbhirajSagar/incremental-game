using UnityEngine;

public enum SFX_EFFECTS
{
    FISH_HIT
}

public class AudioManager : Singleton<AudioManager>
{
    public AudioProfile Profile;
    public SerializableDictionary<SFX_EFFECTS, AudioClip> SFXCollection;

    public AudioSource AmbienceSource;
    public AudioSource BGMSource;
    public AudioSource SFXSource;

    protected override void Awake()
    {
        base.Awake();
        SaveManager.OnDataRefresh += Initialize;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Initialize(GameManager.CurrentSaveData);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        SaveManager.OnDataRefresh -= Initialize;
    }

    public void Initialize(SaveData currentSaveData)
    {
        if (AmbienceSource != null)
        {
            AmbienceSource.volume = currentSaveData.CurAmbienceVolume;
            if (!AmbienceSource.isPlaying) AmbienceSource.Play();
        }

        if (BGMSource != null)
        {
            BGMSource.volume = currentSaveData.CurBGMVolume;
            if (!BGMSource.isPlaying) BGMSource.Play();
        }

        if (SFXSource != null)
        {
            SFXSource.volume = currentSaveData.CurSFXVolume;
        }
    }

    public void PlaySFX(SFX_EFFECTS effect, AudioSource Target, bool VariablePitch = false)
    {
        if (SFXCollection.ContainsKey(effect))
        {
            Target.clip = SFXCollection[effect];
            Target.volume = GameManager.CurrentSaveData.CurSFXVolume;

            if(VariablePitch) 
            Target.pitch = Random.Range(0.8f, 1.2f);
            
            Target.Play();
        }
    }
}