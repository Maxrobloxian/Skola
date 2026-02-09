using UnityEngine;

public static class NoiseSettings
{
    public const float noiseZoom = 0.01f, persistance = .5f, redistributionModifier = 1.2f, exponent = 4;
    public const int octaves = 5, waterHeight = 10;

    public static readonly Vector2Int offsetSeed = new(-100, 3400);
    public static readonly Vector2Int worldSeed = new(0, 0);
    //public static readonly Vector2Int worldSeed = new(Random.Range(0, 10000), Random.Range(0, 10000));
}