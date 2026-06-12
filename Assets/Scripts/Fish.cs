using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Fish : MonoBehaviour, IClickable
{
    public static List<Fish> AllFishes = new();
    [Range(1f, 100f)] public float Speed = 1f;
    [Range(0f, 500f)] public int Health;
    [Range(0, 1000)] public int SellingPrice = 1;

    [Header("REFERENCES")]
    public Canvas HealthbarUICanvas;
    public Image HealthbarFill;
    public ParticleSystem DeathEffect;
    public GameObject Graphics;
    public AudioSource HitSoundSFX;

    [Header("ANIMATION SETTINGS")]
    public float AnimationDuration = 0.5f;
    public float AutoHideHealthbarDuration = 2f;

    [Header("EFFECTS")]
    public ParticleSystem ClickSplashEffect;

    private float CurHealth;
    private Vector3? TargetPos;
    
    private Vector3 originalScale;
    private Tween hitTween;
    private Tween healthbarTween;
    private float LastDamagedTime;
    private bool IsHealthbarVisible;
    private FISH_STATE FishState = FISH_STATE.ALIVE;

    private void Start()
    {
        AllFishes.Add(this);

        originalScale = transform.localScale;
        HealthbarUICanvas.worldCamera = Camera.main;
        
        CurHealth = Health;
        HealthbarFill.fillAmount = 1f;

        HealthbarUICanvas.transform.localScale = Vector3.zero;
        IsHealthbarVisible = false;
    }

    private void OnEnable() => CameraManager.AlwaysFaceCamera.Add(HealthbarUICanvas.transform);
    private void OnDisable() => CameraManager.AlwaysFaceCamera.Remove(HealthbarUICanvas.transform);
    private void OnDestroy() => AllFishes.Remove(this);

    private void Update()
    {
        if (TargetPos != null && FishState == FISH_STATE.ALIVE) 
            transform.LookAt(TargetPos.Value);
            
        if (IsHealthbarVisible && Time.time - LastDamagedTime >= AutoHideHealthbarDuration)
        {
            IsHealthbarVisible = false;
            healthbarTween?.Kill();
            healthbarTween = HealthbarUICanvas.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack).SetLink(gameObject);
        }
    }

    public void SetTargetPos(Vector3 pos)
    {
        TargetPos = pos;
        float durationToReachTarget = Vector3.Distance(transform.position, TargetPos.Value) / Speed;
        transform.DOMove(TargetPos.Value, durationToReachTarget).SetEase(Ease.Linear).SetLink(gameObject);
    }

    public void OnClick()
    {
        if(FishState != FISH_STATE.ALIVE) return;

        Damage();
        LastDamagedTime = Time.time;

        if (!IsHealthbarVisible)
        {
            IsHealthbarVisible = true;
            healthbarTween?.Kill();
            healthbarTween = HealthbarUICanvas.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBounce).SetLink(gameObject);
        }
    }

    private void Damage()
    {
        ClickSplashEffect.Play();
        CameraManager.Instance.TriggerCameraShake(CAMERA_SHAKE.FISH_DAMAGE);
        AudioManager.Instance.PlaySFX(SFX_EFFECTS.FISH_HIT, HitSoundSFX, true);

        hitTween?.Kill();
        transform.localScale = originalScale;
        hitTween = transform.DOShakeScale(0.5f, AnimationDuration, 10, 1f).SetLink(gameObject);

        float damageAmount = GameManager.Session.State.BaseDamage;
        CurHealth = Mathf.Max(CurHealth - damageAmount, 0);
        HealthbarFill.fillAmount = CurHealth / Health;
        
        DamageNumbersGenerator.Instance.Spawn(transform.position, damageAmount);

        if (CurHealth <= 0) Death();
    }

    private void Death()
    {
        FishState = FISH_STATE.DEAD;
        transform.DOKill();
        hitTween?.Kill();
        healthbarTween?.Kill();

        Graphics.SetActive(false);
        HealthbarUICanvas.gameObject.SetActive(false);
        
        // [FIXED] DeathEffect detached but never destroyed. Caused severe memory leak over time.
        DeathEffect.transform.SetParent(null, true);
        DeathEffect.Play();
        
        float effectDuration = DeathEffect.main.duration + DeathEffect.main.startLifetime.constantMax;
        Destroy(DeathEffect.gameObject, effectDuration);

        GameManager.Session.AddMoney(SellingPrice);
        Destroy(gameObject, 1f);
    }

    public void OxygenFinishedDespawn()
    {
        if (FishState == FISH_STATE.DEAD) return;
        FishState = FISH_STATE.DEAD;
        
        transform.DOKill();
        hitTween?.Kill();
        healthbarTween?.Kill();
        transform.DOScale(Vector3.zero, 0.1f).SetEase(Ease.InBounce).OnComplete(() => Destroy(gameObject));
    }
}