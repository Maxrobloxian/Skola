using UnityEngine;
using UnityEngine.InputSystem;

public class UIDraggingManager : MonoBehaviour
{
    [SerializeField] InputActionReference mouseAction;

    private void OnEnable()
    {
        transform.position = mouseAction.action.ReadValue<Vector2>();
    }

    private void Update()
    {
        transform.position = mouseAction.action.ReadValue<Vector2>();
    }
}