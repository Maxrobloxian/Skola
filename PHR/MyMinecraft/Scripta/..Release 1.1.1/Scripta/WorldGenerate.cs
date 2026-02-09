public static class WorldGenerate
{
    internal static void GenWorld(int worldSize, World world)
    {
        for (int x = 0; x < worldSize; x++)
        {
            for (int z = 0; z < worldSize; z++)
            {
#if DEBUG
                GameStats.StartChunkGenLog(new (x, z));
#endif

                ChunkData chunkData = new(x, z);
                ChunkGenerator.Generate(chunkData);

                WorldData.chunkData.Add(chunkData.worldPosition, chunkData);

#if DEBUG
                GameStats.FinishChunkGenLog(new(x, z));
#endif
            }
        }

        foreach (ChunkData chunkData in WorldData.chunkData.Values)
        {
#if DEBUG
            GameStats.StartChunkRenderLog(chunkData.worldPosition);
#endif

            ChunkRenderer chunkRenderer = world.InstantiateChunk(chunkData);
            WorldData.chunkRenderer.Add(chunkData.worldPosition, chunkRenderer);

            chunkRenderer.SetData(chunkData);
            chunkRenderer.RenderChunk(Chunk.GetChunkMeshData(chunkData));

#if DEBUG
            GameStats.FinishChunkRenderLog(chunkData.worldPosition);
#endif
        }

#if DEBUG
        GameStats.FinishWorldLog();
#endif
    }
}