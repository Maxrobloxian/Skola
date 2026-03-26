using UnityEngine;

[CreateAssetMenu(fileName ="NoiseSettings", menuName = "Data/NoiseSettings")]
public class NoiseSettings : ScriptableObject
{
    public float noiseZoom = 0.01f, persistance = .5f, redistributionModifier = 1.2f, exponent = 4;
    public int octaves = 5;

    public Vector2Int offsetSeed = new(-100, 3400);
}