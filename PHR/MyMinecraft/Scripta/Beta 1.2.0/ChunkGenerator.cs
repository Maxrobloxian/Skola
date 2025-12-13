using UnityEngine;

public static class ChunkGenerator
{
    internal static void Generate(ChunkData chunkData)
    {
        for (int x = 0; x < ChunkStats.width; x++)
        {
            for (int z = 0; z < ChunkStats.width; z++)
            {
                float noiseValue = Mathf.PerlinNoise((ChunkStats.seed.x + chunkData.realPosition.x + x) * ChunkStats.noiseScale, (ChunkStats.seed.y + chunkData.realPosition.y + z) * ChunkStats.noiseScale);
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
    }
}