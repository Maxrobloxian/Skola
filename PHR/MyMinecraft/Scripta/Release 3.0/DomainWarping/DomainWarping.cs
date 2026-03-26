using UnityEngine;

public class DomainWarping : MonoBehaviour
{
    [SerializeField] NoiseSettings noiseDomainX, noiseDomainZ;
    [SerializeField] int amplitudeX = 20, amplitudeZ = 20;

    public float GenerateDomainNoise(int x, int z, NoiseSettings noiseSettings)
    {
        Vector2 domaintOffset = GenerateDomainOffset(x, z);
        return Noise.OctavePerlin(x + domaintOffset.x, z + domaintOffset.y, noiseSettings);
    }
    Vector2 GenerateDomainOffset(int x, int z)
    {
        return new(Noise.OctavePerlin(x, z, noiseDomainX) * amplitudeX, Noise.OctavePerlin(x, z, noiseDomainZ) * amplitudeZ);
    }

    public Vector2Int GenerateDomainOffsetInt(int x, int z)
    {
        return Vector2Int.RoundToInt(GenerateDomainOffset(x, z));
    }
}