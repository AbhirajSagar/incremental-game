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
    public bool Enabled
    {
        get
        {
            return CursorImage.enabled;
        }
        set 
        {
            if(value)
            {
                InputManager.Instance.OnMouseMove += FollowMouse;
            }
            else
            {
                InputManager.Instance.OnMouseMove -= FollowMouse;
            }

            CursorImage.enabled = value;
        }
    }

    private void FollowMouse(Vector2 vector)
    {
        Pos = vector;
    }
}