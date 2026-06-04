using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class DamageNumber : MonoBehaviour
{
    public TextMeshPro Label;
    public float DelayInFade = 0.2f;
    public MinMax<float> AnimationDuration;
    public MinMax<Vector3> AnimationDistance;

    private void OnEnable()
    {
        Label.enabled = false;
    }

    public void Initialize(float damage)
    {
        Label.text = ((int) damage).ToString();
        CameraManager.AlwaysFaceCamera.Add(transform);
        Label.enabled = true;

        float animDuration = AnimationDuration.GetRandomValue();
        Vector3 TargetPos = transform.position + AnimationDistance.GetRandomValue();

        transform.DOMove(TargetPos, animDuration); 
        Label.DOFade(0, animDuration).SetDelay(DelayInFade);
    }

    private void OnDestroy()
    {
        CameraManager.AlwaysFaceCamera.Remove(transform);
    }
}
