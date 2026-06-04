using System;
using UnityEngine;

[Serializable]
public class TargetAreaElement : IConfigInitializable
{
    public RectTransform Ring;
    public Material RingProceduralMaterial;
    public float RingRadiusAdjust = 0.5f;

    public Vector2 Position 
    { 
        get => Ring.position;
        set
        {
            Ring.position = value;
        }
    }

    public void Initialize(ConfigManager Config, bool IsUpdate = true)
    {
        RingProceduralMaterial.SetFloat("_Radius", Config.AttackRadius * RingRadiusAdjust);
    }
}