using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Transform eyes;

    // Movemnt
    Vector3 moveDirection;
    Vector2 moveInput, flatVelocity;
    float moveSpeed;

    // Crouching
    const int ignorePlayerMask = -9;
    float playerHalfHeight;
    bool isCrouching;

    // Jump
    bool isGrounded, readyToJump = true;

    Rigidbody rb;
    PlayerMoveStateManager playerMoveStateManager;
    PlayerInputs playerInputs;

    void Awake()
    {
        playerInputs = GetComponent<PlayerInputs>();
        rb = GetComponent<Rigidbody>();

        // Set initial movement values
        playerMoveStateManager = new(this);
    }

    void OnEnable()
    {
        playerInputs.TogglePlayerMovement(true, playerMoveStateManager);
    }

    void OnDisable()
    {
        playerInputs.TogglePlayerMovement(false, playerMoveStateManager);
    }

    void FixedUpdate()
    {
        if (isCrouching && isGrounded && IsZeroMove())
        {
            //print("move: " + moveDirection);
            rb.linearVelocity = Vector3.zero;
            rb.AddForce(50f * moveSpeed * moveDirection, ForceMode.Force);
            rb.linearVelocity = Vector3.zero;
        }
        else rb.AddForce(50f * moveSpeed * moveDirection.normalized, ForceMode.Force);

        rb.linearVelocity = new(rb.linearVelocity.x * 4 * Time.deltaTime, rb.linearVelocity.y, rb.linearVelocity.z * 4 * Time.deltaTime);
    }

    void Update()
    {
        playerHalfHeight = transform.localScale.y * .5f;

        isGrounded = Physics.CheckBox(transform.position + (Vector3.down * playerHalfHeight), new(transform.localScale.x * .49f, .05f, transform.localScale.z * .49f), Quaternion.identity, ignorePlayerMask);

        Move();
        Jump();
        Crouching();
    }

    void Move()
    {
        moveInput = playerInputs.moveAction.action.ReadValue<Vector2>();
        moveDirection = eyes.forward * moveInput.y + eyes.right * moveInput.x;

        Step();

        flatVelocity = new(rb.linearVelocity.x, rb.linearVelocity.z);
        if (flatVelocity.magnitude > moveSpeed)
        {
            flatVelocity = flatVelocity.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(flatVelocity.x, rb.linearVelocity.y, flatVelocity.y);
        }
    }

    void Jump()
    {
        if (playerInputs.jumpAction.action.ReadValue<float>() == 1 && isGrounded && readyToJump)
        {
            rb.linearVelocity = new(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * PlayerSettings.jumpPower, ForceMode.Impulse);

            readyToJump = false;
            Invoke(nameof(ResetJump), PlayerSettings.jumpCooldown);
        }
    }

    void ResetJump()
    {
        readyToJump = true;
    }

    internal Transform GetEyes()
    {
        return eyes;
    }

    internal void ChangeMoveSpeed(float amount)
    {
        moveSpeed = amount;
    }

    internal void ChangeIsCrouching(bool isCrouching)
    {
        this.isCrouching = isCrouching;
    }

    void Crouching()
    {
        if (isCrouching && isGrounded)
        {
            if (!Physics.CheckBox(transform.position + new Vector3(moveInput.x * transform.localScale.x * .2f, -playerHalfHeight, 0), new(transform.localScale.x * .45f, .05f, transform.localScale.z * .48f), Quaternion.identity, ignorePlayerMask))
            {
                moveDirection.x = 0;
            }

            if (!Physics.CheckBox(transform.position + new Vector3(0, -playerHalfHeight, moveInput.y * transform.localScale.z * .2f), new(transform.localScale.x * .48f, .05f, transform.localScale.z * .45f), Quaternion.identity, ignorePlayerMask))
            {
                moveDirection.z = 0;
            }
        }
    }

    bool IsZeroMove()
    {
        if (moveDirection.x == 0 || moveDirection.z == 0) return true;
        return false;
    }

    void Step()
    {
        if (Mathf.Abs(rb.linearVelocity.x) > .05f && Mathf.Abs(rb.linearVelocity.z) > .05f && Physics.BoxCast(transform.position + ((PlayerSettings.stepHeight - playerHalfHeight) * Vector3.up), new(transform.localScale.x * .52f, 0, transform.localScale.z * .52f), Vector3.down, out RaycastHit hit, Quaternion.identity, PlayerSettings.stepHeight - .005f, ignorePlayerMask))
        {
            rb.position = new (transform.position.x, hit.point.y + playerHalfHeight + .05f, transform.position.z);
        }
    }

    //private void OnDrawGizmos()
    //{
        // Crouch
        //Gizmos.color = Color.yellow;
        //Gizmos.DrawCube(transform.position + new Vector3(moveInput.x * transform.localScale.x * .2f, playerHalfHeight, 0), new Vector3(transform.localScale.x * .45f, .05f, transform.localScale.z * .48f) * 2);

        //Gizmos.color = Color.red;
        //Gizmos.DrawCube(transform.position + new Vector3(0, playerHalfHeight, moveInput.y * transform.localScale.z * .2f), new Vector3(transform.localScale.x * .48f, .05f, transform.localScale.z * .45f) * 2);
    //}
}