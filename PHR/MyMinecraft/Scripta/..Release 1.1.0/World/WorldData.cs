using System.Collections.Generic;
using UnityEngine;

public static class WorldData
{
    /// <summary>
    /// worldPosition to chunkData
    /// </summary>
    public readonly static Dictionary<Vector2Int, ChunkData> chunkData = new();
    /// <summary>
    /// worldPosition to chunkRenderer
    /// </summary>
    public readonly static Dictionary<Vector2Int, ChunkRenderer> chunkRenderer = new();
}