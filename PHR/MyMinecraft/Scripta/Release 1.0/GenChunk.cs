using UnityEngine;

public class GenChunk : MonoBehaviour
{
    internal static void GenerateChunk(GameObject chunkPrefab ,Vector2Int chunkPosition)
    {
        ChunkData chunkData = Instantiate(chunkPrefab, new Vector3Int(chunkPosition[0], 0, chunkPosition[1]), Quaternion.Euler(Vector3.zero)).GetComponent<ChunkData>();
        ChunkDictionary.AddChunk(chunkData);

        for (int x = 0; x < ChunkStats.width; x++)
        {
            for (int z = 0; z < ChunkStats.width; z++)
            {
                float noiseValue = Mathf.PerlinNoise((chunkPosition[0] + x + 4992) * ChunkStats.noiseScale, (chunkPosition[1] + z + 4992) * ChunkStats.noiseScale);
                int groundPosition = Mathf.RoundToInt(noiseValue * ChunkStats.height);

                for (int y = 0; y < ChunkStats.height; y++)
                {
                    if (y > groundPosition)
                    {
                        if (y < ChunkStats.waterHeight)
                        {
                            chunkData.SetBlock(new Vector3Int(x, y, z), BlockType.Water);
                        }
                        else
                        {
                            chunkData.SetBlock(new Vector3Int(x, y, z), BlockType.Air);
                        }
                    }
                    else if (y == groundPosition)
                    {
                        chunkData.SetBlock(new Vector3Int(x, y, z), BlockType.Grass_Dirt);
                    }
                    else
                    {
                        chunkData.SetBlock(new Vector3Int(x, y, z), BlockType.Dirt);
                    }
                }
                chunkData.SetBlock(new Vector3Int(x, ChunkStats.height, z), BlockType.Air);
            }
        }
        chunkData.SpawnChunk(true);
    }
}