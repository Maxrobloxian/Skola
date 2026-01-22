using System.Collections.Concurrent;
using UnityEngine;

public static class WorldData
{
    /// <summary>
    /// worldPosition to chunkData
    /// </summary>
    public static readonly ConcurrentDictionary<Vector2Int, ChunkData> chunkData = new();
    /// <summary>
    /// worldPosition to chunkRenderer
    /// </summary>
    public static readonly ConcurrentDictionary<Vector2Int, ChunkRenderer> chunkRenderer = new();
}