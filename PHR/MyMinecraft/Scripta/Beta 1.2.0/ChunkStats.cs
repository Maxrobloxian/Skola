using UnityEngine;

public static class ChunkStats
{
    public const int width = 16, height = 100, waterHeight = 38;
    public const float noiseScale = 0.01f;

    public static readonly Vector2Int seed = new(Random.Range(0, 10000), Random.Range(0, 10000));
}