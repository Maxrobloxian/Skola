using System.Collections.Generic;
using UnityEngine;

public static class WorldSettings
{
    public static World world;

    public static readonly int waterHeight = 8;
    public static readonly Vector2Int worldSeed = new(0, 0);
    //public static readonly Vector2Int worldSeed = new(Random.Range(0, 10000), Random.Range(0, 10000));

    public static NoiseSettings defaultNoise;
    public static List<BiomeLayerHandler> biomeLayerHandlers = new();

    public static DomainWarping domainWarping;
    
    internal static void AddData(NoiseSettings defaultNoise, Transform biomeHandler, DomainWarping domainWarping, World world)
    {
        WorldSettings.defaultNoise = defaultNoise;

        for (int i = 0; i < biomeHandler.childCount; i++)
        {
            biomeLayerHandlers.Add(biomeHandler.GetChild(i).GetChild(0).GetComponent<BiomeLayerHandler>());
        }

        WorldSettings.domainWarping = domainWarping;
        WorldSettings.world = world;
    }
}