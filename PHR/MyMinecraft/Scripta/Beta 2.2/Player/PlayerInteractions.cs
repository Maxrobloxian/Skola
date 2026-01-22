using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractions : MonoBehaviour
{
    UIHotbar uiHotbar;
    PlayerInventory playerInventory;

    [SerializeField] InputActionReference attackAction;
    [SerializeField] InputActionReference interactAction;
    [SerializeField] InputActionReference hotbarScrollAction;

    Camera mainCamera;

    void Awake()
    {
        mainCamera = Camera.main;

        playerInventory = GetComponent<PlayerInventory>();
        uiHotbar = UISettings.uiHotbar;
    }

    void OnEnable()
    {
        attackAction.action.performed += OnAttack;
        interactAction.action.performed += OnInteract;
        hotbarScrollAction.action.performed += OnHotbarScroll;
    }

    void OnDisable()
    {
        attackAction.action.performed -= OnAttack;
        interactAction.action.performed -= OnInteract;
        hotbarScrollAction.action.performed -= OnHotbarScroll;
    }

    void OnAttack(InputAction.CallbackContext context)
    {
        GetHitBlockPos((blockPos, chunkRenderer) => BreakBlock(blockPos, chunkRenderer));
    }

    void OnInteract(InputAction.CallbackContext context)
    {
        GetHitBlockPos((blockPos, chunkRenderer) => PlaceBlock(blockPos, chunkRenderer, playerInventory.GetBlockType(PlayerSettings.selectedSlot)));
    }

    void OnHotbarScroll(InputAction.CallbackContext context)
    {
        float scrollValue = context.ReadValue<float>();
        if (scrollValue > 0)
        {
            PlayerSettings.selectedSlot = (PlayerSettings.selectedSlot + 1) % PlayerSettings.hotbarRowLength;
        }
        else if (scrollValue < 0)
        {
            PlayerSettings.selectedSlot = (PlayerSettings.selectedSlot - 1 + PlayerSettings.hotbarRowLength) % PlayerSettings.hotbarRowLength;
        }

        uiHotbar.HightlightSlot(PlayerSettings.selectedSlot);
    }

    void GetHitBlockPos(Action<Vector3Int, ChunkRenderer> action)
    {
        if (Physics.Raycast(new Ray(mainCamera.transform.position, mainCamera.transform.forward), out RaycastHit hit, PlayerSettings.interactionRange))
        {
            if (hit.collider.TryGetComponent<ChunkRenderer>(out ChunkRenderer chunkRenderer))
            {
                action(Chunk.GetRaycastBlockPosition(hit, chunkRenderer.chunkData.realPosition), chunkRenderer);
            }
        }
    }

    void BreakBlock(Vector3Int blockPos, ChunkRenderer chunkRenderer)
    {
        playerInventory.AddItem(chunkRenderer.chunkData.GetBlock(blockPos), 1, false);
        chunkRenderer.chunkData.SetBlock(blockPos, BlockType.Air);

        Chunk.UpdateChunksNearBlock(blockPos, chunkRenderer);
    }

    void PlaceBlock(Vector3Int blockPos, ChunkRenderer chunkRenderer, BlockType blockType)
    {

        if (playerInventory.GetBlockType(PlayerSettings.selectedSlot) == BlockType.Nothing) return;

        chunkRenderer.chunkData.SetBlock(blockPos, blockType);
        playerInventory.RemoveItem(blockType, 1, false);

        Chunk.UpdateChunksNearBlock(blockPos, chunkRenderer);
    }
}