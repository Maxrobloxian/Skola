using UnityEngine;

public static class Noise
{
    public static float RemapValue(float value, float initialMin, float initialMax, float targetMin, float targetMax)
    {
        return targetMin + (value - initialMin) * (targetMax - targetMin) / (initialMax - initialMin);
    }

    public static float RemapValue01(float value, float targetMin, float targetMax)
    {
        return targetMin + value * (targetMax - targetMin);
    }

    public static int RemapValue01ToInt(float value, float targetMin, float targetMax)
    {
        return (int)RemapValue01(value, targetMin, targetMax);
    }

    public static float Redistribution(float noise)
    {
        return Mathf.Pow(noise * WorldSettings.defaultNoise.redistributionModifier, WorldSettings.defaultNoise.exponent);
    }

    public static float OctavePerlin(float x, float z, NoiseSettings noiseSettings)
    {
        x *= noiseSettings.noiseZoom;
        z *= noiseSettings.noiseZoom;
        x += noiseSettings.noiseZoom;
        z += noiseSettings.noiseZoom;

        float total = 0;
        float frequency = 1;
        float amplitude = 1;
        float amplitudeSum = 0;

        for(int i = 0; i < noiseSettings.octaves; i++)
        {
            total += Mathf.PerlinNoise((noiseSettings.offsetSeed.x + WorldSettings.worldSeed.x + x) * frequency, (noiseSettings.offsetSeed.y + WorldSettings.worldSeed.y + z) * frequency) * amplitude;

            amplitudeSum += amplitude;

            amplitude *= noiseSettings.persistance;
            frequency *= 2;
        }

        return total / amplitudeSum;
    }
}