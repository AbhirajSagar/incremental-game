using System;

[Serializable]
public class CameraShakeSettings
{
    public float Strength;
    public float Duration;

    public CameraShakeSettings(float strength, float duration)
    {
        Strength = strength;
        Duration = duration;
    }
}