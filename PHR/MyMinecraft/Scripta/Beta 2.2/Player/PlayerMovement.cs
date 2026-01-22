using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Transform eyes;

    [Header("Input Actions")]
    [SerializeField] InputActionReference moveAction;
    [SerializeField] InputActionReference jumpAction;
    [SerializeField] InputActionReference sprintAction;
    [SerializeField] InputActionReference crouchAction;
    [SerializeField] InputActionReference crawlAction;

    // Movemnt
    Vector3 moveDirection;
    Vector2 moveInput, flatVelocity;
    internal float moveSpeed;

    // Jump
    float groundRayLenght, jumpCooldown;
    bool isGrounded, readyToJump = true;

    Rigidbody rb;
    PlayerMoveStateManager playerMoveStateManager;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        groundRayLenght = transform.localScale.y * .5f + .02f;

        // Set initial movement values
        playerMoveStateManager = new(this);
        jumpCooldown = PlayerSettings.jumpCooldown;
    }

    void OnEnable()
    {
        sprintAction.action.performed += playerMoveStateManager.SprintStatePerform;
        crouchAction.action.performed += playerMoveStateManager.CrouchStatePerform;
        crawlAction.action.performed += playerMoveStateManager.CrawlStatePerform;

        sprintAction.action.canceled += playerMoveStateManager.SprintStateCancel;
        crouchAction.action.canceled += playerMoveStateManager.CrouchStateCancel;
        crawlAction.action.canceled += playerMoveStateManager.CrawlStateCancel;
    }

    void OnDisable()
    {
        sprintAction.action.performed -= playerMoveStateManager.SprintStatePerform;
        crouchAction.action.performed -= playerMoveStateManager.CrouchStatePerform;
        crawlAction.action.performed -= playerMoveStateManager.CrawlStatePerform;

        sprintAction.action.canceled -= playerMoveStateManager.SprintStateCancel;
        crouchAction.action.canceled -= playerMoveStateManager.CrouchStateCancel;
        crawlAction.action.canceled -= playerMoveStateManager.CrawlStateCancel;
    }

    void FixedUpdate()
    {
        rb.AddForce(50f * moveSpeed * moveDirection.normalized, ForceMode.Force);
        rb.linearVelocity = new(rb.linearVelocity.x * 4 * Time.deltaTime, rb.linearVelocity.y, rb.linearVelocity.z * 4 * Time.deltaTime);
    }

    void Update()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundRayLenght);

        Move();
        Jump();
    }

    void Move()
    {
        moveInput = moveAction.action.ReadValue<Vector2>();
        moveDirection = eyes.forward * moveInput.y + eyes.right * moveInput.x;

        flatVelocity = new(rb.linearVelocity.x, rb.linearVelocity.z);
        if (flatVelocity.magnitude > moveSpeed)
        {
            flatVelocity = flatVelocity.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(flatVelocity.x, rb.linearVelocity.y, flatVelocity.y);
        }
    }

    void Jump()
    {
        if (jumpAction.action.ReadValue<float>() == 1 && isGrounded && readyToJump)
        {
            rb.AddForce(Vector3.up * PlayerSettings.jumpPower, ForceMode.Impulse);

            readyToJump = false;
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    void ResetJump()
    {
        readyToJump = true;
    }
}