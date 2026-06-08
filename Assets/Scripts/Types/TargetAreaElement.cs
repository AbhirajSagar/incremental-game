using System;
using UnityEngine;

[Serializable]
public class TargetAreaElement : IConfigurable
{
    public RectTransform Ring;
    public Material RingProceduralMaterial;
    public float RingRadiusAdjust = 0.5f;

    public Vector2 Position 
    { 
        get => Ring.position;
        set => Ring.position = value;
    }

    public void ApplyConfig(GameConfig config)
    {
        RingProceduralMaterial.SetFloat("_Radius", GameManager.Session.State.AttackRadius * RingRadiusAdjust);
    }
}