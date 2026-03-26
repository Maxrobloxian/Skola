using System.Collections.Generic;
using UnityEngine;

public static class WorldSettings
{
    public static World world;

    public static readonly int waterHeight = 8;
    public static readonly int maxTreeHeight = 45;

    public static Vector2Int worldSeed { get; private set; }

    public static NoiseSettings defaultNoise;
    public static NoiseSettings treeNoise;
    public static List<BiomeLayerHandler> biomeLayerHandlers = new();

    public static DomainWarping domainWarping;
    
    internal static void AddData(NoiseSettings defaultNoise, NoiseSettings treeNoise, Transform biomeHandler, DomainWarping domainWarping, World world)
    {
        WorldSettings.defaultNoise = defaultNoise;
        WorldSettings.treeNoise = treeNoise;

        biomeLayerHandlers.Clear();
        for (int i = 0; i < biomeHandler.childCount; i++)
        {
            biomeLayerHandlers.Add(biomeHandler.GetChild(i).GetChild(0).GetComponent<BiomeLayerHandler>());
        }

        WorldSettings.domainWarping = domainWarping;
        WorldSettings.world = world;
    }

    internal static void SetWorldSeed(Vector2Int seed)
    {
        worldSeed = seed;
    }
}