using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractions : MonoBehaviour
{
    PlayerInventory playerInventory;
    PlayerInputs playerInputs;

    internal Vector3Int interactionTarget;
    Vector3Int placementTarget;
    internal ChunkRenderer interactionTargetChunkRenderer;
    ChunkRenderer placementTargetChunkRenderer;

    bool inMenu;

    Camera mainCamera;

    void Awake()
    {
        playerInputs = GetComponent<PlayerInputs>();
        mainCamera = Camera.main;

        playerInventory = GetComponent<PlayerInventory>();
    }

    void OnEnable()
    {
        playerInputs.TogglePlayerInteractions(true, this);
    }

    void OnDisable()
    {
        playerInputs.TogglePlayerInteractions(false, this);
    }

    void Update()
    {
        GetHitBlockPos(false);
        GetHitBlockPos(true);
    }

    internal void OnAttack(InputAction.CallbackContext context)
    {
        if (!interactionTargetChunkRenderer || inMenu) return;
        
        BreakBlock(interactionTarget, interactionTargetChunkRenderer);
    }

    internal void OnInteract(InputAction.CallbackContext context)
    {
        if (!placementTargetChunkRenderer || inMenu) return;

        PlaceBlock(placementTarget, placementTargetChunkRenderer, playerInventory.GetBlockTypeInHands());
    }

    internal void OnInventoryButton(InputAction.CallbackContext context)
    {
        UISettings.uiPlayerInventory.ToggleInventory();
    }

    internal void OnHotbarScroll(InputAction.CallbackContext context)
    {
        if (inMenu) return;

        UpdateHotbar(PlayerSettings.selectedSlot, false);

        float scrollValue = context.ReadValue<float>();
        if (scrollValue > 0) PlayerSettings.selectedSlot = (PlayerSettings.selectedSlot + 1) % PlayerSettings.hotbarRowLength;
        else if (scrollValue < 0) PlayerSettings.selectedSlot = (PlayerSettings.selectedSlot - 1 + PlayerSettings.hotbarRowLength) % PlayerSettings.hotbarRowLength;

        UpdateHotbar(PlayerSettings.selectedSlot, true);
    }
    internal void OnHotbarButton(InputAction.CallbackContext context)
    {
        if (inMenu) return;

        UpdateHotbar(PlayerSettings.selectedSlot, false);
        PlayerSettings.selectedSlot = (int)context.ReadValue<float>();
        UpdateHotbar(PlayerSettings.selectedSlot, true);
    }

    internal void ChangeInMenu(bool inMenu)
    {
        this.inMenu = inMenu;
    }

    void UpdateHotbar(int slotIndex, bool newSlot)
    {
        if (newSlot) UISettings.uiManager.StartSlotHightlight(UISettings.uiHotbar.uiInventoryData.slotsBackgroundImage[slotIndex]);
        else UISettings.uiManager.StopSlotHightlight(UISettings.uiHotbar.uiInventoryData.slotsBackgroundImage[slotIndex]);
    }

    void GetHitBlockPos(bool placeBlock)
    {
        if (Physics.Raycast(new Ray(mainCamera.transform.position, mainCamera.transform.forward), out RaycastHit hit, PlayerSettings.interactionRange)
            && hit.collider.TryGetComponent<ChunkRenderer>(out ChunkRenderer chunkRenderer))
        {
            if (!placeBlock)
            {
                interactionTarget = Chunk.GetRaycastedBlockPosition(hit, chunkRenderer.chunkData.realPosition);
                interactionTargetChunkRenderer = chunkRenderer;
                return;
            }
            else
            {
                (placementTargetChunkRenderer, placementTarget) = Chunk.GetOutOfBoundChunkRenderer(chunkRenderer, Chunk.GetRaycastedBlockPositionOnSide(hit, chunkRenderer.chunkData.realPosition));
                return;
            }
        }
        if (!placeBlock)
        {
            interactionTarget = -Vector3Int.one;
            interactionTargetChunkRenderer = null;
        }
        else
        {
            placementTarget = -Vector3Int.one;
            placementTargetChunkRenderer = null;
        }
    }

    void BreakBlock(Vector3Int blockPos, ChunkRenderer chunkRenderer)
    {
        BlockType blockType = chunkRenderer.chunkData.GetBlock(blockPos);
        if (blockType == BlockType.Bedrock || blockType == BlockType.Air) return;

        playerInventory.AddItemLooped(PlayerSettings.selectedSlot, blockType, 1, false);
        chunkRenderer.chunkData.SetBlock(blockPos, BlockType.Air);

        Chunk.UpdateChunksNearBlock(blockPos, chunkRenderer);
    }

    void PlaceBlock(Vector3Int blockPos, ChunkRenderer chunkRenderer, BlockType blockType)
    {
        if (blockType == BlockType.Nothing) return;

        chunkRenderer.chunkData.SetBlock(blockPos, blockType);
        playerInventory.RemoveItem(PlayerSettings.selectedSlot, blockType, 1, false);

        Chunk.UpdateChunksNearBlock(blockPos, chunkRenderer);
    }
}