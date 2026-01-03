using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class WorldGenerate
{
    internal static void GenWorld(Vector2Int playerWorldPosition, World world)
    {
        List<Vector2Int> neededChunksDataPos = new();
        for (int x = Mathf.FloorToInt(playerWorldPosition.x - GameSettings.renderDistance - 1); x <= Mathf.FloorToInt(playerWorldPosition.x + GameSettings.renderDistance + 1); x++)
        {
            for (int z = Mathf.FloorToInt(playerWorldPosition.y - GameSettings.renderDistance - 1); z <= Mathf.FloorToInt(playerWorldPosition.y + GameSettings.renderDistance + 1); z++)
            {
#if DEBUG
                PerformanceStats.StartChunkDataLog(new(x, z));
#endif
                WorldData.chunkData.TryGetValue(new(x, z), out ChunkData chunkData);
                if (chunkData == null)
                {
                    chunkData = new(x, z);
                    ChunkGenerator.Generate(chunkData);

                    WorldData.chunkData.Add(chunkData.worldPosition, chunkData);
                }
                neededChunksDataPos.Add(chunkData.worldPosition);
#if DEBUG
                PerformanceStats.FinishChunkDataLog(new(x, z));
#endif
            }
        }
#if DEBUG
        PerformanceStats.AllChunksDataLog();
#endif
        List<Vector2Int> neededChunksRenderersPos = new();
        for (int x = Mathf.FloorToInt(playerWorldPosition.x - GameSettings.renderDistance); x <= Mathf.FloorToInt(playerWorldPosition.x + GameSettings.renderDistance); x++)
        {
            for (int z = Mathf.FloorToInt(playerWorldPosition.y - GameSettings.renderDistance); z <= Mathf.FloorToInt(playerWorldPosition.y + GameSettings.renderDistance); z++)
            {
                ChunkData chunkData = WorldData.chunkData[new Vector2Int(x, z)];
#if DEBUG
                PerformanceStats.StartChunkRenderLog(chunkData.worldPosition);
#endif
                WorldData.chunkRenderer.TryGetValue(chunkData.worldPosition, out ChunkRenderer chunkRenderer);
                if (chunkRenderer == null)
                {
                    chunkRenderer = world.InstantiateChunk(chunkData);
                    WorldData.chunkRenderer.Add(chunkData.worldPosition, chunkRenderer);

                    chunkRenderer.SetData(chunkData);
                    chunkRenderer.RenderChunk(Chunk.GetChunkMeshData(chunkData));
                }
                neededChunksRenderersPos.Add(chunkData.worldPosition);
#if DEBUG
                PerformanceStats.FinishChunkRenderLog(chunkData.worldPosition);
#endif
            }
        }
#if DEBUG
        PerformanceStats.FinishWorldLog();
#endif

        ClearUnusedChunkData(neededChunksDataPos);
        ClearUnusedRenderedChunks(neededChunksRenderersPos, world);
    }

    static void ClearUnusedChunkData(List<Vector2Int> neededChunks)
    {
        List<Vector2Int> unneededChunks = new(WorldData.chunkData.Keys.Where(pos => neededChunks.Contains(pos) == false).ToList());

        foreach (Vector2Int worldPosition in unneededChunks)
        {
            WorldData.chunkData.Remove(worldPosition);
        }
    }

    static void ClearUnusedRenderedChunks(List<Vector2Int> neededChunks, World world)
    {
        List<Vector2Int> unneededChunks = new(WorldData.chunkRenderer.Keys.Where(pos => neededChunks.Contains(pos) == false).ToList());
        
        //List<Vector2Int> unneededChunks = new();
        //foreach (Vector2Int worldPosition in WorldData.chunkRenderer.Keys.Where(pos => neededChunks.Contains(pos) == false).ToList())
        //{
        //    if (WorldData.chunkRenderer.ContainsKey(worldPosition))
        //    {
        //        unneededChunks.Add(worldPosition);
        //    }
        //}                                                                           Case of error change to this
                
        foreach (Vector2Int worldPosition in unneededChunks)
        {
            world.DestroyChunk(WorldData.chunkRenderer[worldPosition].gameObject);
            WorldData.chunkRenderer.Remove(worldPosition);
        }
    }
}