using System.Diagnostics;
using UnityEngine;

public class GameStats : MonoBehaviour
{
    static readonly Stopwatch gameLoad = new();
    static readonly double[,] chunkLoadTime = new double[15*15,2];

    // World
    private void Awake()
    {
        gameLoad.Start();
    }
    public static void FinishWorldLog()
    {
        gameLoad.Stop();

        double averageLoad = -chunkLoadTime[0, 0], averageRender = -chunkLoadTime[0, 1];
        string header = "Release1.1\nWorld size: 15x15\nChunks: 225\n\n";

        System.Text.StringBuilder fullLog = new();
        fullLog.AppendLine(header + "World Load Time: " + (float)gameLoad.Elapsed.TotalSeconds + "s\n");
        
        for (int i = 0; i < chunkLoadTime.GetLength(0); i++)
        {
            averageLoad += chunkLoadTime[i, 0];
            averageRender += chunkLoadTime[i, 1];

            fullLog.AppendLine("Chunk " + i + " Load: " + (float)chunkLoadTime[i, 0] + "ms");
            fullLog.AppendLine("Chunk " + i + " Render: " + (float)chunkLoadTime[i, 1]+ "ms");
        }
        averageLoad /= 225;
        averageRender /= 225;

        System.IO.File.WriteAllText(Application.dataPath + "/log.txt", string.Format(header + "World Load Time: {0:F3}s\n\nChunk Load Time\nFirst: {1:F2}ms   Average: {2:F2}ms\n\nChunk Render Time\nFirst: {3:F2}ms   Average: {4:F2}ms", gameLoad.Elapsed.TotalSeconds, chunkLoadTime[0, 0], averageLoad, chunkLoadTime[0, 1], averageRender));
        System.IO.File.WriteAllText(Application.dataPath + "/full log.txt", fullLog.ToString());
    }
    // World


    // Chunk
    // ChunkGen
    public static void StartChunkGenLog(Vector2Int chunkPos)
    {
        chunkLoadTime[chunkPos.x * 15 + chunkPos.y, 0] = gameLoad.Elapsed.TotalMilliseconds;
    }
    public static void FinishChunkGenLog(Vector2Int chunkPos)
    {
        chunkLoadTime[chunkPos.x * 15 + chunkPos.y, 0] = gameLoad.Elapsed.TotalMilliseconds - chunkLoadTime[chunkPos.x * 15 + chunkPos.y, 0];
    }
    // ChunkGen

    // ChunkRender
    public static void StartChunkRenderLog(Vector2Int chunkPos)
    {
        chunkLoadTime[chunkPos.x * 15 + chunkPos.y, 1] = gameLoad.Elapsed.TotalMilliseconds;
    }
    public static void FinishChunkRenderLog(Vector2Int chunkPos)
    {
        chunkLoadTime[chunkPos.x * 15 + chunkPos.y, 1] = gameLoad.Elapsed.TotalMilliseconds - chunkLoadTime[chunkPos.x * 15 + chunkPos.y, 1];
    }
    // ChunkRender
    // Chunk
}