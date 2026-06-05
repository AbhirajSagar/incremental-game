using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class DamageNumber : MonoBehaviour
{
    public TextMeshPro Label;
    public float DelayInFade = 0.2f;
    
    public MinMaxFloat AnimationDuration;
    public MinMaxVector3 AnimationDistance;

    private Action<DamageNumber> onCompleteCallback;

    private void OnEnable()
    {
        CameraManager.AlwaysFaceCamera.Add(transform);
    }

    private void OnDisable()
    {
        CameraManager.AlwaysFaceCamera.Remove(transform);
    }

    public void Initialize(float damage, Action<DamageNumber> onComplete)
    {
        onCompleteCallback = onComplete;
        Label.text = ((int)damage).ToString();
        
        // Reset label from previous pooled uses
        Label.alpha = 1f;
        
        float animDuration = AnimationDuration.GetRandomValue();
        Vector3 targetPos = transform.position + AnimationDistance.GetRandomValue();

        transform.DOMove(targetPos, animDuration).SetLink(gameObject); 
        Label.DOFade(0, animDuration).SetDelay(DelayInFade).SetLink(gameObject).OnComplete(Deactivate);
    }

    private void Deactivate()
    {
        onCompleteCallback?.Invoke(this);
    }
}