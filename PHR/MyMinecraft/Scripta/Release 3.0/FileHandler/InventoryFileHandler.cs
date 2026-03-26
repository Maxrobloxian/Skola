using System.IO;
using UnityEngine;

public static class InventoryFileHandler
{
    internal static void Save()
    {
        string folderPath = $"{Application.streamingAssetsPath}/Worlds/{GameSettings.worldRealName}/";

        if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

        using BinaryWriter writer = new(new FileStream($"{folderPath}inventoryData.dat", FileMode.Create));

        InventoryData inventorySlots = PlayerSettings.playerInventory.inventorySlots;
        InventoryData offhandSlots = PlayerSettings.playerInventory.offhandSlots;
        InventoryData draggingSlot = PlayerSettings.playerInventory.draggingSlot;

        writer.Write(GetInventoryItemSlotsCount(inventorySlots));
        for (int i = 0; i < inventorySlots.slots.Length; i++)
        {
            if (inventorySlots.slots[i].blockType != BlockType.Nothing)
            {
                writer.Write(i);
                writer.Write((byte)inventorySlots.slots[i].blockType);
                writer.Write((byte)inventorySlots.slots[i].amount);
            }
        }
        writer.Write(GetInventoryItemSlotsCount(offhandSlots));
        for (int i = 0; i < offhandSlots.slots.Length; i++)
        {
            if (offhandSlots.slots[i].blockType != BlockType.Nothing)
            {
                writer.Write(i);
                writer.Write((byte)offhandSlots.slots[i].blockType);
                writer.Write((byte)offhandSlots.slots[i].amount);
            }
        }
        writer.Write((byte)draggingSlot.slots[0].blockType);
        writer.Write((byte)draggingSlot.slots[0].amount);

        writer.Write((byte)UISettings.uiPlayerInventory.oldSlotIndex);
    }

    internal static void Load(InventoryData inventorySlots, InventoryData offhandSlots, InventoryData draggingSlot)
    {
        string filePath = $"{Application.streamingAssetsPath}/Worlds/{GameSettings.worldRealName}/inventoryData.dat";

        if (!File.Exists(filePath)) return;

        using BinaryReader reader = new(new FileStream(filePath, FileMode.Open));

        int inventoryDataCount = reader.ReadInt32();
        for (int i = 0; i < inventoryDataCount; i++)
        {
            int slot = reader.ReadInt32();
            inventorySlots.slots[slot].blockType = (BlockType)reader.ReadByte();
            inventorySlots.slots[slot].amount = reader.ReadByte();
        }
        inventoryDataCount = reader.ReadInt32();
        for (int i = 0; i < inventoryDataCount; i++)
        {
            int slot = reader.ReadInt32();
            offhandSlots.slots[slot].blockType = (BlockType)reader.ReadByte();
            offhandSlots.slots[slot].amount = reader.ReadByte();
        }
        draggingSlot.slots[0].blockType = (BlockType)reader.ReadByte();
        draggingSlot.slots[0].amount = reader.ReadByte();

        UISettings.uiPlayerInventory.LoadOldSlotIndex(reader.ReadByte());
    }

    static int GetInventoryItemSlotsCount(InventoryData inventoryData)
    {
        int inventoryDataCount = 0;
        for (int i = 0; i < inventoryData.slots.Length; i++)
        {
            if (inventoryData.slots[i].blockType != BlockType.Nothing)
            {
                inventoryDataCount++;
            }
        }
        return inventoryDataCount;
    }
}