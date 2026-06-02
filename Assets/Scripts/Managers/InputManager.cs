using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public bool IsInputEnabled = true;

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && IsInputEnabled)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.TryGetComponent(out IClickable clickable))
                {
                    clickable.OnClick();
                }
            }
        }
    }
}