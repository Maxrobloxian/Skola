using System.IO;
using UnityEngine;

public static class ChunkFileHandler
{
    internal static void SaveChunk(ChunkData chunkData)
    {
        using BinaryWriter writer = new(new FileStream($"{Application.streamingAssetsPath}/Worlds/{GameSettings.worldRealName}/{chunkData.worldPosition.x}_{chunkData.worldPosition.y}.dat", FileMode.Create));

        writer.Write(chunkData.hasTrees);
        writer.Write(chunkData.updateTrees);

        for (int x = 0; x < ChunkSettings.width; x++)
        {
            for (int y = 0; y < ChunkSettings.height + 1; y++)
            {
                for (int z = 0; z < ChunkSettings.width; z++)
                {
                    writer.Write((byte)chunkData.GetBlock(x, y, z));
                }
            }
        }
    }

    internal static ChunkData Load(Vector2Int worldPos)
    {
        string filePath = $"{Application.streamingAssetsPath}/Worlds/{GameSettings.worldRealName}/{worldPos.x}_{worldPos.y}.dat";

        if (!File.Exists(filePath)) return null;

        using BinaryReader reader = new(new FileStream(filePath, FileMode.Open));

        ChunkData chunkData = new(worldPos.x, worldPos.y, reader.ReadBoolean(), reader.ReadBoolean());

        for (int x = 0; x < ChunkSettings.width; x++)
        {
            for (int y = 0; y < ChunkSettings.height + 1; y++)
            {
                for (int z = 0; z < ChunkSettings.width; z++)
                {
                    chunkData.SetBlock(x, y, z, (BlockType)reader.ReadByte());
                }
            }
        }

        chunkData.CancelIsUpdated();

        return chunkData;
    }

    internal static void Save()
    {
        string savePath = $"{Application.streamingAssetsPath}/Worlds/{GameSettings.worldRealName}/";
        if (!Directory.Exists(savePath)) Directory.CreateDirectory(savePath);

        foreach (ChunkData chunkData in WorldData.chunkData.Values)
        {
            if (chunkData.isUpdated) SaveChunk(chunkData);
        }
    }
}