using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    internal InventoryData inventorySlots { get; private set; }
    internal InventoryData offhandSlots { get; private set; }
    internal InventoryData draggingSlot { get; private set; }

    private void Awake()
    {
        inventorySlots = gameObject.AddComponent<InventoryData>().Initialize(PlayerSettings.inventoryRowLength, PlayerSettings.inventoryRows);
        offhandSlots = gameObject.AddComponent<InventoryData>().Initialize(PlayerSettings.offhandRowLength, 1);
        draggingSlot = gameObject.AddComponent<InventoryData>().Initialize(1, 1);
    }

    internal void AddItemLooped(int selectedSlot, BlockType blockType, int amount, bool offHand)
    {
        if (!offHand) AddItemLooped(selectedSlot, blockType, amount, inventorySlots.slots, UISettings.uiPlayerInventory.uiInventoryData);
        else AddItemLooped(selectedSlot, blockType, amount, offhandSlots.slots, UISettings.uiHotbar.uiInventoryData);
    }
    void AddItemLooped(int selectedSlot, BlockType blockType, int amount, InventorySlot[] slots, UIInventoryData uiInventoryData)
    {
        int firstEmptySlot = -1;

        if (slots[selectedSlot].blockType == blockType && slots[selectedSlot].amount < PlayerSettings.maxAmountPerSlot)
        {
            (slots[selectedSlot], amount) = AddingLogic(selectedSlot,slots[selectedSlot], blockType, amount, uiInventoryData);
            if (amount == 0) return;
        }
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].blockType == blockType && slots[i].amount < PlayerSettings.maxAmountPerSlot)
            {
                (slots[i], amount) = AddingLogic(i, slots[i], blockType, amount, uiInventoryData);
                if (amount == 0) return;
            }
            else if (firstEmptySlot == -1 && slots[i].blockType == BlockType.Nothing)
            {
                firstEmptySlot = i;
            }
        }
        AddItemToEmptyLooped(firstEmptySlot, blockType, amount, slots, uiInventoryData);
    }
    void AddItemToEmptyLooped(int firstEmptySlot, BlockType blockType, int amount, InventorySlot[] slots, UIInventoryData uiInventoryData)
    {
        for (int i = firstEmptySlot; i < slots.Length; i++)
        {
            if (slots[i].blockType == BlockType.Nothing)
            {
                (slots[i], amount) = AddingLogic(i, slots[i], blockType, amount, uiInventoryData);
                if (amount == 0) return;
            }
        }
    }

    (InventorySlot, int) AddingLogic(int slotIndex, InventorySlot slot, BlockType blockType, int amount, UIInventoryData uiInventoryData)
    {
        if (slot.amount + amount > PlayerSettings.maxAmountPerSlot)
        {
            amount -= PlayerSettings.maxAmountPerSlot - slot.amount;
            slot = MaxInventorySlot(slot, blockType);
        }
        else
        {
            slot = ChangeInventorySlot(slot, blockType, amount);
            amount = 0;
        }
        UpdateUI(slotIndex, slot, uiInventoryData);
        return (slot, amount);
    }

    internal void RemoveItem(int slotIndex, BlockType blockType, int amount, bool offHand)
    {
        if (!offHand) inventorySlots.slots[slotIndex] = RemoveItem(slotIndex, blockType, amount, inventorySlots.slots[slotIndex], UISettings.uiPlayerInventory.uiInventoryData);
        else offhandSlots.slots[slotIndex] = RemoveItem(slotIndex, blockType, amount, offhandSlots.slots[slotIndex], UISettings.uiHotbar.uiInventoryData);
    }
    InventorySlot RemoveItem(int slotIndex, BlockType blockType, int amount, InventorySlot slot, UIInventoryData uiInventoryData)
    {
        if (slot.amount > amount)
        {
            slot = ChangeInventorySlot(slot, blockType, -amount);
        }
        else if (slot.amount - amount == 0)
        {
            slot = ResetInventorySlot(slot);
        }
        else
        {
            Debug.LogError("Not enough items in inventory to remove.");
        }

        UpdateUI(slotIndex, slot, uiInventoryData);
        return slot;
    }

    InventorySlot ChangeInventorySlot(InventorySlot slot, BlockType blockType, int amount)
    {
        slot.blockType = blockType;
        slot.amount += amount;
        return slot;
    }
    InventorySlot MaxInventorySlot(InventorySlot slot, BlockType blockType)
    {
        slot.amount = PlayerSettings.maxAmountPerSlot;
        return ChangeInventorySlot(slot, blockType, 0);
    }
    InventorySlot ResetInventorySlot(InventorySlot slot)
    {
        slot.amount = 0;
        return ChangeInventorySlot(slot, BlockType.Nothing, 0);
    }

    internal BlockType GetBlockType(int slotIndex, bool offHand)
    {
        if (!offHand) return inventorySlots.slots[slotIndex].blockType;
        return offhandSlots.slots[slotIndex].blockType;
    }
    internal BlockType GetBlockTypeInHands()
    {
        BlockType blockType = GetBlockType(PlayerSettings.selectedSlot, false);
        if (blockType == BlockType.Nothing) blockType = GetBlockType(PlayerSettings.selectedoffhandSlot, true);
        return blockType;
    }

    internal int GetBlockQuantity(int slotIndex, bool offHand)
    {
        if (!offHand) return inventorySlots.slots[slotIndex].amount;
        return offhandSlots.slots[slotIndex].amount;
    }

    void UpdateUI(int slotIndex, InventorySlot slot, UIInventoryData uiInventoryData)
    {
        UISettings.uiManager.UpdateSlot(uiInventoryData.slotsImage[slotIndex], uiInventoryData.slotsText[slotIndex], slot.blockType, slot.amount);

        if (slotIndex < PlayerSettings.hotbarRowLength && uiInventoryData == UISettings.uiPlayerInventory.uiInventoryData)
            UISettings.uiManager.UpdateSlot(UISettings.uiHotbar.uiInventoryData.slotsImage[slotIndex], UISettings.uiHotbar.uiInventoryData.slotsText[slotIndex], slot.blockType, slot.amount);
    }

    internal void MoveFromDraggingSlot(InventoryData inventoryData, int slotIndex, int amount, UIInventoryData uiInventoryData, UIInventoryData uiDraggedSlotData)
    {
        int leftoverAmount;
        (inventoryData.slots[slotIndex], leftoverAmount) = AddingLogic(slotIndex, inventoryData.slots[slotIndex], draggingSlot.slots[0].blockType, amount, uiInventoryData);

        draggingSlot.slots[0] = RemoveItem(0, draggingSlot.slots[0].blockType, amount - leftoverAmount, draggingSlot.slots[0], uiDraggedSlotData);
    }
    internal void MoveToDraggingSlot(InventoryData inventoryData, int slotIndex, int amount, UIInventoryData uiInventoryData, UIInventoryData uiDraggedSlotData)
    {
        int leftoverAmount;
        (draggingSlot.slots[0], leftoverAmount) = AddingLogic(0, draggingSlot.slots[0], inventoryData.slots[slotIndex].blockType, amount, uiDraggedSlotData);

        inventoryData.slots[slotIndex] = RemoveItem(slotIndex, inventoryData.slots[slotIndex].blockType, amount - leftoverAmount, inventoryData.slots[slotIndex], uiInventoryData);
    }
    internal void SwapDraggingAndInventory(InventoryData inventoryData, int slotIndex, UIInventoryData uiInventoryData)
    {
        (draggingSlot.slots[0], inventorySlots.slots[slotIndex]) = (inventorySlots.slots[slotIndex], draggingSlot.slots[0]);

        UpdateUI(slotIndex, inventoryData.slots[slotIndex], uiInventoryData);
    }
}

struct InventorySlot
{
    internal BlockType blockType;
    internal int amount;
}