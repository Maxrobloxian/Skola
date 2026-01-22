using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PerformanceStats : MonoBehaviour
{
    static readonly Stopwatch gameLoad = new();
    static readonly Dictionary<Vector2Int, double> chunkDataLoadTime = new();
    static readonly Dictionary<Vector2Int, double> chunkRenderLoadTime = new();
    static double chunkGenTime;

    // World
    private void Awake()
    {
        if (GameSettings.renderDistance == 7)
        {
            print("Performance Stats Enabled");
            gameLoad.Start();
        }
        else
        {
            throw new System.Exception("Render distance not 7 / display value 8");
        }
    }
    public static void FinishWorldLog()
    {
        gameLoad.Stop();

#if DEBUG
        if (!GameObject.FindWithTag("GameManager").GetComponent<GameManager>().enablePerformanceStats) return;
#endif

        List<double> chunkDataTimes = new();
        foreach (double time in chunkDataLoadTime.Values)
        {
            chunkDataTimes.Add(time);
        }

        List<double> chunkRenderTimes = new();
        foreach (double time in chunkRenderLoadTime.Values)
        {
            chunkRenderTimes.Add(time);
        }

        System.Text.StringBuilder fullLog = new();
        double averageChunkData = -chunkDataTimes[0];
        double averageChunkRender = -chunkRenderTimes[0];

        string header = "Release2.0\nWorld size: 15x15\nData Chunks: 289\nRender Chunks: 225\n\n";

        fullLog.AppendLine(header + "World Load Time: " + (float)gameLoad.Elapsed.TotalSeconds + "s");
        fullLog.AppendLine("World Data Time: " + (float)chunkGenTime + "s");
        fullLog.AppendLine("World Render Time: " + (float)(gameLoad.Elapsed.TotalSeconds - chunkGenTime) + "s\n");

        for (int i = 0; i < chunkRenderTimes.Count; i++)
        {
            averageChunkData += chunkDataTimes[i];
            averageChunkRender += chunkRenderTimes[i];

            fullLog.AppendLine("Chunk " + i + " Data: " + (float)chunkDataTimes[i] + "ms");
            fullLog.AppendLine("Chunk " + i + " Render: " + (float)chunkRenderTimes[i] + "ms");
        }

        fullLog.AppendLine("-------------------------------------------------------------------");
        for (int i = chunkRenderTimes.Count; i < chunkDataTimes.Count; i++)
        {
            averageChunkData += chunkDataTimes[i];

            fullLog.AppendLine("Chunk " + i + " Data: " + (float)chunkDataTimes[i] + "ms");
        }

        averageChunkData /= chunkDataTimes.Count;
        averageChunkRender /= chunkRenderTimes.Count;

        System.IO.File.WriteAllText(Application.dataPath + "/log.log", string.Format(header + "World Data Time: {0:F3}s\nWorld Gen Time: {1:F3}s\nWorld Render Time: {2:F3}s\n\nChunk Data Time\nFirst: {3:F2}ms   Average: {4:F2}ms\n\nChunk Render Time\nFirst: {5:F2}ms   Average: {6:F2}ms", gameLoad.Elapsed.TotalSeconds, chunkGenTime, gameLoad.Elapsed.TotalSeconds - chunkGenTime, chunkDataTimes[0], averageChunkData, chunkRenderTimes[0], averageChunkRender));
        System.IO.File.WriteAllText(Application.dataPath + "/full log.log", fullLog.ToString());

#if DEBUG
        GameObject.FindWithTag("GameManager").GetComponent<GameManager>().DestroyPerformanceStats();
#endif
    }
    // World


    // Chunk
    // ChunkGen
    public static void StartChunkDataLog(Vector2Int chunkPos)
    {
        chunkDataLoadTime.TryAdd(chunkPos, gameLoad.Elapsed.TotalMilliseconds);
    }
    public static void FinishChunkDataLog(Vector2Int chunkPos)
    {
        chunkDataLoadTime[chunkPos] = gameLoad.Elapsed.TotalMilliseconds - chunkDataLoadTime[chunkPos];
    }
    public static void AllChunksDataLog()
    {
        chunkGenTime = gameLoad.Elapsed.TotalSeconds;
    }
    // ChunkGen

    // ChunkRender
    public static void StartChunkRenderLog(Vector2Int chunkPos)
    {
        chunkRenderLoadTime.TryAdd(chunkPos, gameLoad.Elapsed.TotalMilliseconds);
    }
    public static void FinishChunkRenderLog(Vector2Int chunkPos)
    {
        chunkRenderLoadTime[chunkPos] = gameLoad.Elapsed.TotalMilliseconds - chunkRenderLoadTime[chunkPos];
    }
    // ChunkRender
    // Chunk
}