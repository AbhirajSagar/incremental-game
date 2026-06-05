using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
    public bool IsInputEnabled = true;
    public LayerMask DetectLayer;
    private Vector3 HitPoint = Vector3.zero;

    public Action<Vector2> OnMouseMove;

    // CACHE: Prevents allocating memory (Garbage Collection) on every click!
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
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            HitPoint = hit.point;
        }

        if (!IsInputEnabled || !Mouse.current.leftButton.wasPressedThisFrame) return;

        float attackRadius = GameManager.Instance.Config.AttackRadius;
        int hitCount = Physics.OverlapSphereNonAlloc(hit.point, attackRadius, hitColliders, DetectLayer);

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
        if (!Application.isPlaying || GameManager.Instance == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(HitPoint, GameManager.Instance.Config.AttackRadius);
    }
}