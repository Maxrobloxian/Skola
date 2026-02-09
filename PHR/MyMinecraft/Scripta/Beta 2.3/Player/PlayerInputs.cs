using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    [Header("Player Movement")]
    [SerializeField] internal InputActionReference moveAction;
    [SerializeField] internal InputActionReference jumpAction;
    [SerializeField] InputActionReference sprintAction;
    [SerializeField] InputActionReference crouchAction;
    [SerializeField] InputActionReference crawlAction;

    [Header("Player Camera")]
    [SerializeField] internal InputActionReference lookAction;

    [Header("Player Interactions")]
    [SerializeField] InputActionReference attackAction;
    [SerializeField] InputActionReference interactAction;
    [SerializeField] InputActionReference hotbarScrollAction;
    [SerializeField] InputActionReference hotbarButtonAction;
    [SerializeField] InputActionReference inventoryAction;


    [Header("Debug")]
    [SerializeField] InputActionReference chunkBordersAction;
    [SerializeField] InputActionReference debugScreenAction;

    // Player
    internal void TogglePlayerMovement(bool isEnabled, PlayerMoveStateManager playerMoveStateManager)
    {
        if (isEnabled)
        {
            sprintAction.action.performed += playerMoveStateManager.SprintStatePerform;
            crouchAction.action.performed += playerMoveStateManager.CrouchStatePerform;
            crawlAction.action.performed += playerMoveStateManager.CrawlStatePerform;

            sprintAction.action.canceled += playerMoveStateManager.SprintStateCancel;
            crouchAction.action.canceled += playerMoveStateManager.CrouchStateCancel;
            crawlAction.action.canceled += playerMoveStateManager.CrawlStateCancel;
        }
        else
        {
            sprintAction.action.performed -= playerMoveStateManager.SprintStatePerform;
            crouchAction.action.performed -= playerMoveStateManager.CrouchStatePerform;
            crawlAction.action.performed -= playerMoveStateManager.CrawlStatePerform;

            sprintAction.action.canceled -= playerMoveStateManager.SprintStateCancel;
            crouchAction.action.canceled -= playerMoveStateManager.CrouchStateCancel;
            crawlAction.action.canceled -= playerMoveStateManager.CrawlStateCancel;
        }
    }    
    internal void TogglePlayerInteractions(bool isEnabled, PlayerInteractions playerInteractions)
    {
        if (isEnabled)
        {
            attackAction.action.performed += playerInteractions.OnAttack;
            interactAction.action.performed += playerInteractions.OnInteract;
            hotbarScrollAction.action.performed += playerInteractions.OnHotbarScroll;
            hotbarButtonAction.action.performed += playerInteractions.OnHotbarButton;
            inventoryAction.action.performed += playerInteractions.OnInventoryButton;
        }
        else
        {
            attackAction.action.performed -= playerInteractions.OnAttack;
            interactAction.action.performed -= playerInteractions.OnInteract;
            hotbarScrollAction.action.performed -= playerInteractions.OnHotbarScroll;
            hotbarButtonAction.action.performed -= playerInteractions.OnHotbarButton;
            inventoryAction.action.performed -= playerInteractions.OnInventoryButton;
        }
    }

    // Debug
    internal void ToggleDebugManager(bool isEnabled, DebugManager debugManager)
    {
        if (isEnabled)
        {
            chunkBordersAction.action.performed += debugManager.ToggleChunkBorders;
            debugScreenAction.action.performed += debugManager.ToggleDebugScreen;
        }
        else
        {
            chunkBordersAction.action.performed -= debugManager.ToggleChunkBorders;
            debugScreenAction.action.performed -= debugManager.ToggleDebugScreen;
        }
    }
}