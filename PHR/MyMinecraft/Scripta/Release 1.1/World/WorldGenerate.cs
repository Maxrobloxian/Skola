public static class WorldGenerate
{
    internal static void GenWorld(int worldSize, World world)
    {
        for (int x = 0; x < worldSize; x++)
        {
            for (int z = 0; z < worldSize; z++)
            {
                ChunkData chunkData = new(x, z);
                ChunkGenerator.Generate(chunkData);

                WorldData.chunkData.Add(chunkData.worldPosition, chunkData);
            }
        }

        foreach (ChunkData chunkData in WorldData.chunkData.Values)
        {
            ChunkRenderer chunkRenderer = world.InstantiateChunk(chunkData);
            WorldData.chunkRenderer.Add(chunkData.worldPosition, chunkRenderer);

            chunkRenderer.SetData(chunkData);
            chunkRenderer.RenderChunk(Chunk.GetChunkMeshData(chunkData));
        }
    }
}