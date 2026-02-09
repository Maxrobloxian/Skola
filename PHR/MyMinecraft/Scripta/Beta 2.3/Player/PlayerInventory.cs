using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    readonly InventorySlot[] inventorySlots = new InventorySlot[PlayerSettings.inventoryRowLength * PlayerSettings.inventoryRows];
    readonly InventorySlot[] offhandSlots = new InventorySlot[PlayerSettings.offhandRowLength];

    internal void AddItem(int selectedSlot, BlockType blockType, int amount, bool offHand)
    {
        if (!offHand) AddItem(selectedSlot, blockType, amount, inventorySlots);
        else AddItem(selectedSlot, blockType, amount, offhandSlots);
    }
    void AddItem(int selectedSlot, BlockType blockType, int amount, InventorySlot[] slots)
    {
        int firstEmptySlot = -1;

        if (slots[selectedSlot].blockType == blockType && slots[selectedSlot].amount < PlayerSettings.maxAmountPerSlot)
        {
            (slots[selectedSlot], amount) = AddingLogic(selectedSlot,slots[selectedSlot], blockType, amount);
            if (amount == 0) return;
        }
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].blockType == blockType && slots[i].amount < PlayerSettings.maxAmountPerSlot)
            {
                (slots[i], amount) = AddingLogic(i, slots[i], blockType, amount);
                if (amount == 0) return;
            }
            else if (firstEmptySlot == -1 && slots[i].blockType == BlockType.Nothing)
            {
                firstEmptySlot = i;
            }
        }
        AddItemToEmpty(firstEmptySlot, blockType, amount, slots);
    }
    void AddItemToEmpty(int firstEmptySlot, BlockType blockType, int amount, InventorySlot[] slots)
    {
        for (int i = firstEmptySlot; i < slots.Length; i++)
        {
            if (slots[i].blockType == BlockType.Nothing)
            {
                (slots[i], amount) = AddingLogic(i, slots[i], blockType, amount);
                if (amount == 0) return;
            }
        }
    }

    (InventorySlot, int) AddingLogic(int slotIndex, InventorySlot slot, BlockType blockType, int amount)
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
        UpdateUI(slotIndex, slot);
        return (slot, amount);
    }

    internal void RemoveItem(int slotIndex, BlockType blockType, int amount, bool offHand)
    {
        if (!offHand) inventorySlots[slotIndex] = RemoveItem(slotIndex, blockType, amount, inventorySlots[slotIndex]);
        else offhandSlots[slotIndex] = RemoveItem(slotIndex, blockType, amount, offhandSlots[slotIndex]);
    }
    InventorySlot RemoveItem(int slotIndex, BlockType blockType, int amount, InventorySlot slot)
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

        UpdateUI(slotIndex, slot);
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
        if (!offHand) return inventorySlots[slotIndex].blockType;
        return offhandSlots[slotIndex].blockType;
    }
    internal BlockType GetBlockTypeInHands()
    {
        BlockType blockType = GetBlockType(PlayerSettings.selectedSlot, false);
        if (blockType == BlockType.Nothing) blockType = GetBlockType(PlayerSettings.selectedoffhandSlot, true);
        return blockType;
    }

    internal int GetBlockQuantity(int slotIndex, bool offHand)
    {
        if (!offHand) return inventorySlots[slotIndex].amount;
        return offhandSlots[slotIndex].amount;
    }

    void UpdateUI(int slotIndexe, InventorySlot slot)
    {
        UISettings.uiManager.UpdateSlot(UISettings.uiPlayerInventory.GetSlotText(slotIndexe), slot.blockType, slot.amount);

        if (slotIndexe < PlayerSettings.hotbarRowLength) UISettings.uiManager.UpdateSlot(UISettings.uiHotbar.GetSlotText(slotIndexe), slot.blockType, slot.amount);
    }
}

struct InventorySlot
{
    internal BlockType blockType;
    internal int amount;
}