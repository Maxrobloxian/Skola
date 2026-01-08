using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] InputActionReference lookAction;

    Vector2 lookInput;
    Transform mainCamera;
    float xAxis;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;

        mainCamera = Camera.main.transform;
        mainCamera.SetParent(transform);
        mainCamera.SetLocalPositionAndRotation(Vector3.zero, transform.parent.rotation);
        mainCamera.localScale = Vector3.one;
    }

    private void Update()
    {
        lookInput = GameSettings.sensitivity * Time.deltaTime * lookAction.action.ReadValue<Vector2>();

        xAxis -= lookInput.y;
        xAxis = Mathf.Clamp(xAxis, -90f, 90f);

        transform.Rotate(Vector3.up * lookInput.x);
        mainCamera.localRotation = Quaternion.Euler(xAxis, 0, 0);
    }
}