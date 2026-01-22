public static class ChunkGenerator
{
    internal static void Generate(ChunkData chunkData)
    {
        for (int x = 0; x < ChunkSettings.width; x++)
        {
            for (int z = 0; z < ChunkSettings.width; z++)
            {
                // float terrainHeight = WorldSettings.domainWarping.GenerateDomainNoise(x, z);
                // terrainHeight = Noise.Redistribution(terrainHeight, noiseSettings);
                // int surfaceHeight = Noise.RemapValue01ToInt(terrainHeight, 0, ChunkStats.height);
                // ==============================
                int groundPosition = Noise.RemapValue01ToInt(Noise.Redistribution(WorldSettings.domainWarping.GenerateDomainNoise(chunkData.realPosition.x + x, chunkData.realPosition.y + z, WorldSettings.defaultNoise)), 0, ChunkSettings.height);

                for (int y = 0; y < ChunkSettings.height; y++)
                {
                    WorldSettings.biomeLayerHandlers[0].Handle(chunkData, x, y, z, groundPosition);
                }

                for (int i = 1; i < WorldSettings.biomeLayerHandlers.Count; i++)
                {
                    WorldSettings.biomeLayerHandlers[i].Handle(chunkData, x, 0, z, groundPosition);
                }

                chunkData.SetBlock(x, ChunkSettings.height, z, BlockType.Air);
            }
        }
    }
}