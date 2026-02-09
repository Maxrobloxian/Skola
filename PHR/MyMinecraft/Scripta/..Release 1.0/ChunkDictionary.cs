using System.Collections.Generic;
using UnityEngine;

public static class ChunkDictionary
{
    public readonly static Dictionary<Vector2Int, ChunkData> chunk = new();

    public static void AddChunk(ChunkData chunkData)
    {
        if (!chunk.ContainsKey(chunkData.worldPosition))
        {
            chunk.Add(chunkData.worldPosition, chunkData);
        }
    }
}