using System;

[Serializable]
public class AudioProfile
{
    public float BackgroundMusicVolume;
    public float SFXVolume;
    public float AmbienceVolume;

    public AudioProfile(float bgm, float sfx, float amb)
    {
        BackgroundMusicVolume = bgm;
        SFXVolume = sfx;
        AmbienceVolume = amb;
    }
}