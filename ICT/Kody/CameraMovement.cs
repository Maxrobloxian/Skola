using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // Tvori float se kterym kontoluju sychlost camery
    [SerializeField] private float sensX;
    [SerializeField] private float sensY;

    // Bere poziti a rotaci objectu ktery sem vytvoril
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform player;

    // Float ktery nemuzu videt ve hre ale jenom uvnitr kodu, zajistuje jak se kamera bude tocit
    float xRotation;
    float yRotation;
    
    // Zajisti ze je cursor uprostred obrazovky a nevyditelna ( ja sem pak udelal custom cursor pomoci UI)
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Generuje float ktery zahycuje informace o axe X a Y
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        // Spujim floaty dohromady a tvorim limit (aby se camera netocila prez krk cloveka)
        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);// <----- to dela ten limit

        // Tady otacim kameru a cloveka
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        player.transform.rotation = orientation.rotation;

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        transform.position = new Vector3(orientation.position.x, orientation.position.y + .86f, orientation.position.z);
    }
}