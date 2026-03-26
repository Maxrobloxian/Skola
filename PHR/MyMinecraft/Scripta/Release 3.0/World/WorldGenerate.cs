using System;
using System.Collections;
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
        StopAllCoroutines();

        List<Vector2Int> neededChunksDataPos = new();
        List<Vector2Int> neededChunksRenderersPos = new();
        
        LoopFromPlayer(playerWorldPosition, OptionsSettings.realRenderDistance, pos =>
        {
            neededChunksDataPos.Add(pos);
            neededChunksRenderersPos.Add(pos);
        });
        LoopFromPlayer(playerWorldPosition, OptionsSettings.realRenderDistance + 1, pos => neededChunksDataPos.Add(pos), OptionsSettings.realRenderDistance + 1);

        ClearUnusedChunkData(neededChunksDataPos);
        ClearUnusedRenderedChunks(neededChunksRenderersPos);

        try
        {
            await GenerateDatas(neededChunksDataPos);
        }
        catch (Exception) { }

        GenerateTrees(neededChunksRenderersPos);

        try
        {
            StartCoroutine(GenerateChunks(neededChunksRenderersPos));
        }
        catch (Exception) { }
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

    async Task GenerateData(int worldPosX, int worldPosZ)
    {
        await Task.Run(() =>
        {
            cts.Token.ThrowIfCancellationRequested();

            WorldData.chunkData.TryGetValue(new(worldPosX, worldPosZ), out ChunkData chunkData);
            if (chunkData == null)
            {
                // Load file or gen new
                chunkData = ChunkFileHandler.Load(new Vector2Int(worldPosX, worldPosZ));
                if (chunkData == null)
                {
                    chunkData = new(worldPosX, worldPosZ);
                    ChunkGenerator.Generate(chunkData, false);
                }

                WorldData.chunkData.TryAdd(chunkData.worldPosition, chunkData);
            }
        }, cts.Token);
    }

    IEnumerator GenerateChunks(List<Vector2Int> neededChunksRenderersPos)
    {
        foreach (Vector2Int pos in neededChunksRenderersPos)
        {
            WorldData.chunkRenderer.TryGetValue(pos, out ChunkRenderer chunkRenderer);
            if (chunkRenderer == null)
            {
                WorldData.chunkData.TryGetValue(new Vector2Int(pos.x, pos.y), out ChunkData chunkData);
                if (chunkData == null) continue;
                chunkRenderer = worldRenderer.RenderChunk(chunkData, Chunk.GetChunkMeshData(chunkData));

                WorldData.chunkRenderer.TryAdd(chunkData.worldPosition, chunkRenderer);
                chunkRenderer.chunkData.SetTreesUpdate(false);

                yield return new WaitForFixedUpdate();
            }
            else if (chunkRenderer.chunkData.updateTrees)
            {
                chunkRenderer.chunkData.SetTreesUpdate(false);
                chunkRenderer.RenderChunk(Chunk.GetChunkMeshData(chunkRenderer.chunkData));
                yield return new WaitForFixedUpdate();
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

    void GenerateTrees(List<Vector2Int> neededChunksRenderersPos)
    {
        foreach (Vector2Int pos in neededChunksRenderersPos)
        {
            WorldData.chunkData.TryGetValue(pos, out ChunkData chunkData);
            if (!chunkData.hasTrees)
            {
                ChunkGenerator.Generate(chunkData, true);
            }
        }
    }

    void ClearUnusedChunkData(List<Vector2Int> neededChunks)
    {
        List<Vector2Int> unneededChunks = new(WorldData.chunkData.Keys.Where(pos => neededChunks.Contains(pos) == false).ToList());

        foreach (Vector2Int worldPosition in unneededChunks)
        {
            WorldData.chunkData.TryGetValue(worldPosition, out ChunkData chunkData);
            if (chunkData.isUpdated) ChunkFileHandler.SaveChunk(chunkData);

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