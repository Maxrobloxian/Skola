using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Ruzny floaty kterymi kontroluju pohyb cloveka
    [Header("Movement")]//  <----- Header mi rozdeluje informace po sekcich
    [SerializeField] private float moveSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float sprintSpeed;

    [Header("Jumping")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float airMultiplier;
    bool readyToJump;

    [Header("Ground Drag")]
    [SerializeField] private float groundDrag;

    // Tady delam nejake kustom klavesy
    [Header("Keybinds")]
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;

    [Header("Ground Check")]
    [SerializeField] private float playerHeight;
    [SerializeField] private bool isGrounded;

    // Tady propojuju smer kam se diva kamera se clovekem
    [Header("Options")]
    [SerializeField] private Transform orientation;

    private float horizontalInput;
    private float verticalInput;

    // Vector3 je neco jako skratka osy X,Y,Z
    Vector3 moveDirection;

    // Rigidbody je generuje gravitaci a pak to ze muzu cloveka posouvat
    Rigidbody rb;

    // Vsehno co se deje ve voidu Start se pripravy pred startem hry
    private void Start()
    {
        // Zajistuju ze rigidbody ma nejakou hodnotu a ze se nebude preklopovat (padal na zem)
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        moveSpeed = walkSpeed;

        // Activuju bool
        readyToJump = true;
    }

    // Vsehno co se deje ve voidu Update se deje kolem do kola (neco jako infinite loop)
    private void Update()
    {
        // Kontroluje jestly se clovek dotyka zemne
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * .5f + .2f);

        // Propojuju voidy mezy sebou
        MyInput();
        SpeedControl();

        // Kdy je clovek na zemy tak se muze zastavit, kdy je ve vzduhu nemuze
        if (isGrounded) { rb.drag = groundDrag; }
        else { rb.drag = 0; }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        // Tvorim osy X,Y
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // Spojuju tlacitka a jejich funkce
        if (Input.GetKey(jumpKey) && readyToJump && isGrounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
        if (Input.GetKey(sprintKey)) { moveSpeed = sprintSpeed; }
        if (Input.GetKeyUp(sprintKey)) { moveSpeed = walkSpeed; }
    }

    private void MovePlayer()
    {
        // Smer ve kterem se bude clovek hybat
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // Meni rychlost podle toho jestly je clovek ve vzduhu nebo ne
        if (isGrounded) { rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force); }
        else if (!isGrounded) { rb.AddForce(moveDirection.normalized * moveSpeed * 10f * 1.2f, ForceMode.Force); }
    }

    // Zajistuju ze clovek neprekroci urcenou rychlost
    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    // Kdy je void Jump pouzity, clovek skace
    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    // Urcuje to ze de skocit vic nez jednou
    private void ResetJump()
    {
        readyToJump = true;
    }
}