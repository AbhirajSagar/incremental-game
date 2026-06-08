using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
    public bool IsInputEnabled = true;
    public LayerMask DetectLayer;
    private Vector3 HitPoint = Vector3.zero;

    public Action<Vector2> OnMouseMove;
    private Collider[] hitColliders = new Collider[30];

    protected override void Awake()
    {
        base.Awake();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void Update()
    {
        Vector2 MousePos = Mouse.current.position.ReadValue();
        OnMouseMove?.Invoke(MousePos);

        Ray ray = Camera.main.ScreenPointToRay(MousePos);
        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, DetectLayer))
        {
            HitPoint = hit.point;
        }
        else
        {
            // [FIXED] If raycast missed the background (clicked outside bounds), HitPoint kept its last position.
            // Using a mathematical plane intersect ensures HitPoint remains accurate based on the camera angle.
            Plane fallbackPlane = new Plane(Vector3.forward, Vector3.zero);
            if (fallbackPlane.Raycast(ray, out float distance))
            {
                HitPoint = ray.GetPoint(distance);
            }
        }

        if (!IsInputEnabled || !Mouse.current.leftButton.wasPressedThisFrame) return;

        float attackRadius = GameManager.Session.State.AttackRadius;
        int hitCount = Physics.OverlapSphereNonAlloc(HitPoint, attackRadius, hitColliders, DetectLayer);

        for (int i = 0; i < hitCount; i++)
        {
            if (hitColliders[i].TryGetComponent(out IClickable clickable))
            {
                clickable.OnClick();
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying || GameManager.Instance == null || GameManager.Session == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(HitPoint, GameManager.Session.State.AttackRadius);
    }
}