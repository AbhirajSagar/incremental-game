using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class CursorSystem
{
    public RectTransform CursorRect;
    public Image CursorImage;
    
    public Vector2 Pos 
    { 
        get => CursorRect.position;
        set => CursorRect.position = value;
    }

    private bool _isEnabled;
    public bool Enabled
    {
        get => _isEnabled;
        set 
        {
            if (_isEnabled == value) return; // [FIXED] Prevent duplicated delegate sub/unsub memory leaks
            _isEnabled = value;
            
            if(_isEnabled)
            {
                if(InputManager.Instance != null) InputManager.Instance.OnMouseMove += FollowMouse;
            }
            else
            {
                if(InputManager.Instance != null) InputManager.Instance.OnMouseMove -= FollowMouse;
            }

            if (CursorImage != null) CursorImage.enabled = _isEnabled;
        }
    }

    private void FollowMouse(Vector2 vector) => Pos = vector;
}