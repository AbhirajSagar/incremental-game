using System;
using DG.Tweening;
using UnityEngine;

public class Fish : MonoBehaviour, IClickable
{
    public float Speed = 1f;
    private Vector3? TargetPos;
    private float DurationToReachTarget;

    private void Update()
    {
        if(TargetPos == null) return;
        
        transform.DOMove(TargetPos.Value, DurationToReachTarget).SetEase(Ease.Linear).SetLink(gameObject);
        transform.LookAt(TargetPos.Value);
    }

    public void SetTargetPos(Vector3 pos)
    {
        TargetPos = pos;
        DurationToReachTarget = Vector3.Distance(transform.position, TargetPos.Value)