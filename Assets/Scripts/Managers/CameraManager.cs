using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : Singleton<CameraManager>
{
    public static List<Transform> AlwaysFaceCamera = new();

    public Camera MainCamera;
    public SerializableDictionary<CAMERA_SHAKE, CameraShakeSettings> CameraShakeSettings;
    
    private bool IsShaking = false;
    private CAMERA_SHAKE CurrentShakeType = CAMERA_SHAKE.NONE;

    protected override void Awake()
    {
        base.Awake();
        AlwaysFaceCamera.Clear(); 
    }

    private void LateUpdate()
    {
        for(int i = 0; i < AlwaysFaceCamera.Count; i++)
        {
            if (AlwaysFaceCamera[i] != null)
                AlwaysFaceCamera[i].rotation = MainCamera.transform.rotation;
        }

        transform.LookAt(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()));
    }

    public void TriggerCameraShake(CAMERA_SHAKE ShakeType)
    {
        if(ShakeType == CAMERA_SHAKE.NONE) return;
        if(IsShaking)
        {
            if(ShakeType <= CurrentShakeType) return;
            else MainCamera.DOComplete();
        }

        if (CameraShakeSettings.TryGetValue(ShakeType, out CameraShakeSettings settings))
        {
            MainCamera.DOShakePosition(settings.Duration, settings.Strength).SetLink(gameObject).OnComplete(() =>
            {
                IsShaking = false;
                CurrentShakeType = CAMERA_SHAKE.NONE;
            });
            IsShaking = true;
            CurrentShakeType = ShakeType;
        }
    }
}