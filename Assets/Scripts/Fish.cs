using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

public class Fish : MonoBehaviour, IClickable
{
    public static List<Fish> AllFishes = new();
    [Range(1f, 100f)] public float Speed = 1f;
    [Range(0f, 500f)] public int Health;

    [Header("REFERENCES")]
    public Canvas HealthbarUICanvas;
    public Image HealthbarFill;
    public ParticleSystem DeathEffect;
    public GameObject Graphics;

    [Header("ANIMATION SETTINGS")]
    public float AnimationDuration = 0.5f;
    public Vector3 SquishStretchAmount = new Vector3(0.3f, -0.3f, 0.3f);
    public float AutoHideHealthbarDuration = 2f;

    [Header("EFFECTS")]
    public ParticleSystem ClickSplashEffect;

    private float CurHealth;
    private Vector3? TargetPos;
    private float DurationToReachTarget;

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
        CameraManager.AlwaysFaceCamera.Add(HealthbarUICanvas.transform);

        CurHealth = Health;
        HealthbarFill.fillAmount = 1f;

        HealthbarUICanvas.transform.localScale = Vector3.zero;
        IsHealthbarVisible = false;
    }

    private void Update()
    {
        if (TargetPos != null) transform.LookAt(TargetPos.Value);
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
        DurationToReachTarget = Vector3.Distance(transform.position, TargetPos.Value) / Speed;
        transform.DOMove(TargetPos.Value, DurationToReachTarget).SetEase(Ease.Linear).SetLink(gameObject);
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

        hitTween?.Kill();
        transform.localScale = originalScale;
        hitTween = transform.DOShakeScale(0.5f, AnimationDuration, 10, 1f).SetLink(gameObject);

        CurHealth = Mathf.Max(CurHealth - ConfigManager.Instance.Damage, 0);
        HealthbarFill.fillAmount = CurHealth / Health;
        DamageNumbersGenerator.CreateDamageNumber(transform.position, ConfigManager.Instance.Damage);

        if (CurHealth == 0)
        {
            Death();
        }
    }

    private void Death()
    {
        FishState = FISH_STATE.DEAD;

        hitTween?.Kill();
        healthbarTween?.Kill();

        Graphics.SetActive(false);
        HealthbarUICanvas.gameObject.SetActive(false);
        
        DeathEffect.transform.SetParent(null, true);
        DeathEffect.Play();
        Destroy(gameObject, 1f);
    }

    private void OnDestroy()
    {
        AllFishes.Remove(this);
        CameraManager.AlwaysFaceCamera.Remove(HealthbarUICanvas.transform);
    }
}