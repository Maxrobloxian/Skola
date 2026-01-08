using UnityEngine.InputSystem;

public class PlayerMoveStateManager
{
    MovementState movementState;
    readonly PlayerMovement playerMovement;
    readonly PlayerCharacter playerCharacter;

    /// <summary>
    /// Set initial movement speed
    /// </summary>
    public PlayerMoveStateManager(PlayerMovement playerMovement)
    {
        this.playerMovement = playerMovement;
        playerCharacter = playerMovement.transform.GetComponent<PlayerCharacter>();
        DoStuff(movementState);
    }

    public enum MovementState
    {
        Walking,
        Sprinting,
        Crouching,
        Crawling
    }

    // Perform states
    internal void SprintStatePerform(InputAction.CallbackContext context)
    {
        if (movementState == MovementState.Walking)
        {
            DoStuff(MovementState.Sprinting);
        }
    }
    internal void CrouchStatePerform(InputAction.CallbackContext context)
    {
        // Add head check later
        if (movementState != MovementState.Sprinting || movementState != MovementState.Crawling)
        {
            DoStuff(MovementState.Crouching);
        }
    }
    internal void CrawlStatePerform(InputAction.CallbackContext context)
    {
        // Add head check later
        if (movementState != MovementState.Sprinting)
        {
            DoStuff(MovementState.Crawling);
        }
    }

    // Cancel states
    internal void SprintStateCancel(InputAction.CallbackContext context)
    {
        if (movementState == MovementState.Sprinting)
        {
            DoStuff(MovementState.Walking);
        }
    }
    internal void CrouchStateCancel(InputAction.CallbackContext context)
    {
        // Add head check later
        if (movementState == MovementState.Crouching)
        {
            DoStuff(MovementState.Walking);
        }
    }
    internal void CrawlStateCancel(InputAction.CallbackContext context)
    {
        // Add head check later
        if (movementState == MovementState.Crawling)
        {
            DoStuff(MovementState.Walking);
        }
    }
    // ------------------------------------------------------

    void DoStuff(MovementState movementState)
    {
        this.movementState = movementState;

        playerMovement.moveSpeed = movementState switch
        {
            MovementState.Walking => PlayerSettings.walkSpeed,
            MovementState.Sprinting => PlayerSettings.walkSpeed * PlayerSettings.sprintMultiplier,
            MovementState.Crouching => PlayerSettings.walkSpeed * PlayerSettings.crouchMultiplier,
            MovementState.Crawling => PlayerSettings.walkSpeed * PlayerSettings.crawlMultiplier,
            _ => throw new System.Exception("Invalid movement state")
        };

        playerCharacter.ChangeCharacterSize(movementState switch
        {
            MovementState.Walking => PlayerSettings.walkingHeight,
            MovementState.Sprinting => PlayerSettings.walkingHeight,
            MovementState.Crouching => PlayerSettings.crouchingHeight,
            MovementState.Crawling => PlayerSettings.crawlingHeight,
            _ => throw new System.Exception("Invalid movement state")
        });
    }
}