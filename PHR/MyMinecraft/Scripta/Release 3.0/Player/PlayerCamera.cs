using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] PlayerInputs playerInputs;

    Vector2 lookInput;
    Transform mainCamera;
    float xAxis;

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    private void OnDestroy()
    {
        UISettings.parentCounter.RemovePlayerEvents();
    }

    private void Awake()
    {
        mainCamera = Camera.main.transform;
        mainCamera.SetParent(transform);
        mainCamera.SetLocalPositionAndRotation(Vector3.zero, transform.parent.rotation);
        mainCamera.localScale = Vector3.one;

        UISettings.parentCounter.AddPlayerEvents();

        var saveData = PlayerFileHandler.LoadRotations();
        if (saveData != null)
        {
            transform.localRotation = Quaternion.Euler(0, saveData.Value.Item1, 0);
            mainCamera.localRotation = Quaternion.Euler(saveData.Value.Item2, 0, 0);

            xAxis = saveData.Value.Item2;
        }
    }

    private void Update()
    {
        lookInput = OptionsSettings.sensitivity * Time.deltaTime * playerInputs.lookAction.action.ReadValue<Vector2>();

        xAxis -= lookInput.y;
        xAxis = Mathf.Clamp(xAxis, -90f, 90f);

        transform.Rotate(Vector3.up * lookInput.x);
        mainCamera.localRotation = Quaternion.Euler(xAxis, 0, 0);
    }
}