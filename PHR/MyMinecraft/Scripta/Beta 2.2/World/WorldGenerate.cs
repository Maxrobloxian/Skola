using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class WorldGenerate : MonoBehaviour
{
    WorldRenderer worldRenderer;

    readonly CancellationTokenSource cts = new();

    void Awake()
    {
        worldRenderer = GetComponent<WorldRenderer>();
    }

    void OnDisable()
    {
        cts.Cancel();
    }

    internal async Task GenWorld(Vector2Int playerWorldPosition)
    {
        List<Vector2Int> neededChunksDataPos = new();
        List<Vector2Int> neededChunksRenderersPos = new();
        
        LoopFromPlayer(playerWorldPosition, GameSettings.renderDistance, pos =>
        {
            neededChunksDataPos.Add(pos);
            neededChunksRenderersPos.Add(pos);
        });
        LoopFromPlayer(playerWorldPosition, GameSettings.renderDistance + 1, pos => neededChunksDataPos.Add(pos), GameSettings.renderDistance + 1);

        ClearUnusedChunkData(neededChunksDataPos);
        ClearUnusedRenderedChunks(neededChunksRenderersPos);

        try
        {
            await GenerateDatas(neededChunksDataPos);
        }
        catch (Exception)
        {
            print("Task cancelled");
        }

#if DEBUG
        PerformanceStats.AllChunksDataLog();
#endif

        try
        {
            await GenerateChunks(neededChunksRenderersPos);
        }
        catch (Exception)
        {
            print("Task cancelled");
        }

#if DEBUG
        PerformanceStats.FinishWorldLog();
#endif
    }

    void LoopFromPlayer(Vector2Int playerWorldPosition, int renderDistance, Action<Vector2Int> actionToPerform, int startAtLoopNr = 1)
    {
        if (startAtLoopNr == 1) actionToPerform(playerWorldPosition);

        for (int r = startAtLoopNr; r <= renderDistance; r++)
        {
            // --- Top Edge (Left to Right) ---
            for (int x = -r + playerWorldPosition.x; x <= r + playerWorldPosition.x; x++)
            {
                actionToPerform(new(x, r + playerWorldPosition.y)); // Fixed Z (top)
            }

            // --- Right Edge (Top to Bottom) ---
            // Start at r-1 to avoid recounting the corner
            for (int z = r - 1 + playerWorldPosition.y; z >= -r + playerWorldPosition.y; z--)
            {
                actionToPerform(new(r + playerWorldPosition.x, z)); // Fixed X (right)
            }

            // --- Bottom Edge (Right to Left) ---
            for (int x = r - 1 + playerWorldPosition.x; x >= -r + playerWorldPosition.x; x--)
            {
                actionToPerform(new(x, -r + playerWorldPosition.y)); // Fixed Z (bottom)
            }

            // --- Left Edge (Bottom to Top) ---
            for (int z = -r + 1 + playerWorldPosition.y; z < r + playerWorldPosition.y; z++)
            {
                actionToPerform(new(-r + playerWorldPosition.x, z)); // Fixed X (left)
            }
        }
    }

    async Task GenerateData(int x, int z)
    {
#if DEBUG
        PerformanceStats.StartChunkDataLog(new(x, z));
#endif
        await Task.Run(() =>
        {
            cts.Token.ThrowIfCancellationRequested();

            WorldData.chunkData.TryGetValue(new(x, z), out ChunkData chunkData);
            if (chunkData == null)
            {
                chunkData = new(x, z);
                ChunkGenerator.Generate(chunkData);

                WorldData.chunkData.TryAdd(chunkData.worldPosition, chunkData);
            }
        }, cts.Token);
#if DEBUG
        PerformanceStats.FinishChunkDataLog(new(x, z));
#endif
    }

    async Task GenerateChunk(int x, int z)
    {
#if DEBUG
        PerformanceStats.StartChunkRenderLog(new(x, z));
#endif

        ChunkData chunkData = WorldData.chunkData[new Vector2Int(x, z)];
        WorldData.chunkRenderer.TryAdd(chunkData.worldPosition, worldRenderer.RenderChunk(chunkData, await Task.Run(() => Chunk.GetChunkMeshDataThreaded(chunkData, cts), cts.Token)));

#if DEBUG
        PerformanceStats.FinishChunkRenderLog(new(x, z));
#endif
    }

    async Task GenerateChunks(List<Vector2Int> neededChunksRenderersPos)
    {
        foreach (Vector2Int pos in neededChunksRenderersPos)
        {
            cts.Token.ThrowIfCancellationRequested();

            WorldData.chunkRenderer.TryGetValue(pos, out ChunkRenderer chunkRenderer);
            if (chunkRenderer == null)
            {
                await GenerateChunk(pos.x, pos.y);
#if DEBUG
                await Task.Yield();
#else
                await Task.Delay(TimeSpan.FromSeconds(0.01f));
#endif
            }
        }
    }

    async Task GenerateDatas(List<Vector2Int> neededChunksDataPos)
    {
        foreach (Vector2Int pos in neededChunksDataPos)
        {
            await GenerateData(pos.x, pos.y);
        }
    }

    void ClearUnusedChunkData(List<Vector2Int> neededChunks)
    {
        List<Vector2Int> unneededChunks = new(WorldData.chunkData.Keys.Where(pos => neededChunks.Contains(pos) == false).ToList());

        foreach (Vector2Int worldPosition in unneededChunks)
        {
            WorldData.chunkData.TryRemove(worldPosition, out _);
        }
    }

    void ClearUnusedRenderedChunks(List<Vector2Int> neededChunks)
    {
        List<Vector2Int> unneededChunks = new(WorldData.chunkRenderer.Keys.Where(pos => neededChunks.Contains(pos) == false).ToList());

        foreach (Vector2Int worldPosition in unneededChunks)
        {
            WorldData.chunkRenderer.TryRemove(worldPosition, out ChunkRenderer chunkRenderer);
            worldRenderer.RemoveChunk(chunkRenderer);
        }
    }
}