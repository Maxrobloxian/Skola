using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    readonly InventorySlot[] inventorySlots = new InventorySlot[PlayerSettings.inventoryRowLength];
    readonly InventorySlot[] offhandSlots = new InventorySlot[PlayerSettings.offhandRowLength];

    internal void AddItem(BlockType blockType, int amount, bool offHand)
    {
        if (!offHand) AddItem(blockType, amount, inventorySlots);
        else AddItem(blockType, amount, offhandSlots);
    }
    InventorySlot[] AddItem(BlockType blockType, int amount, InventorySlot[] slots)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].blockType == blockType || slots[i].blockType == BlockType.Nothing)
            {
                slots[i] = ChangeInventorySlot(i, slots[i], blockType, amount);
                return slots;
            }
        }
        return slots;
    }

    internal void RemoveItem(BlockType blockType, int amount, bool offHand)
    {
        if (!offHand) RemoveItem(blockType, amount, inventorySlots);
        else RemoveItem(blockType, amount, offhandSlots);
    }
    InventorySlot[] RemoveItem(BlockType blockType, int amount, InventorySlot[] slots)
    {
        amount = -amount;
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].blockType == blockType)
            {
                if (slots[i].amount > -amount)
                {
                    slots[i] = ChangeInventorySlot(i, slots[i], blockType, amount);
                }
                else if (slots[i].amount + amount == 0)
                {
                    slots[i] = ResetInventorySlot(i, slots[i]);
                }
                else
                {
                    Debug.LogError("Not enough items in inventory to remove.");
                }
                return slots;
            }
        }
        return slots;
    }

    InventorySlot ChangeInventorySlot(int slotIndex, InventorySlot slot, BlockType blockType, int amount)
    {
        slot.blockType = blockType;
        slot.amount += amount;

        if (slotIndex <= PlayerSettings.hotbarRowLength) UISettings.uiHotbar.UpdateSlot(slotIndex, slot.blockType, slot.amount);
        return slot;
    }

    InventorySlot ResetInventorySlot(int slotIndex, InventorySlot slot)
    {
        slot.amount = 0;
        return ChangeInventorySlot(slotIndex, slot, BlockType.Nothing, 0);
    }

    internal BlockType GetBlockType(int slotIndex)
    {
        return inventorySlots[slotIndex].blockType;
    }

    internal int GetBlockQuantity(int slotIndex)
    {
        return inventorySlots[slotIndex].amount;
    }
}

struct InventorySlot
{
    internal BlockType blockType;
    internal int amount;
}