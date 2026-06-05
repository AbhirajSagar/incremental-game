using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
    public bool IsInputEnabled = true;
    public LayerMask DetectLayer;
    private Vector3 HitPoint = Vector3.zero;

    public Action<Vector2> OnMouseMove;

    protected override void Awake()
    {
        base.Awake();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        Vector2 MousePos = Mouse.current.position.ReadValue();
        OnMouseMove?.Invoke(MousePos);

        if (Physics.Raycast(Camera.main.ScreenPointToRay(MousePos), out RaycastHit hit))
        {
            HitPoint = hit.point;
        }

        if (!IsInputEnabled) return;
        if (!Mouse.current.leftButton.wasPressedThisFrame) return;

        Collider[] insideDetection = new Collider[30];
        int hitCount = Physics.OverlapSphereNonAlloc(hit.point, ConfigManager.Instance.AttackRadius, insideDetection, DetectLayer);

        for (int i = 0; i < hitCount; i++)
        {
            if (insideDetection[i].TryGetComponent(out IClickable clickable))
                clickable.OnClick();
        }
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying || ConfigManager.Instance == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(HitPoint, ConfigManager.Instance.AttackRadius);
    }
}