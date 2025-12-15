using UnityEngine;

public static class ChunkGenerator
{
    internal static void Generate(ChunkData chunkData)
    {
        for (int x = 0; x < ChunkStats.width; x++)
        {
            for (int z = 0; z < ChunkStats.width; z++)
            {
                int groundPosition = GetSurfaceHeight(chunkData.realPosition.x + x, chunkData.realPosition.y + z);

                for (int y = 0; y < ChunkStats.height; y++)
                {
                    if (y > groundPosition)
                    {
                        if (y < NoiseSettings.waterHeight)
                        {
                            chunkData.SetBlock(new Vector3Int(x, y, z), BlockType.Water);
                        }
                        else
                        {
                            chunkData.SetBlock(new Vector3Int(x, y, z), BlockType.Air);
                        }
                    }
                    else if (y == groundPosition && y < NoiseSettings.waterHeight)
                    {
                        chunkData.SetBlock(new Vector3Int(x, y, z), BlockType.Sand);
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

        int GetSurfaceHeight(int x, int z)
        {
            float terrainHeight = Noise.OctavePerlin(x, z);
            terrainHeight = Noise.Redistribution(terrainHeight);
            int surfaceHeight = Noise.RemapValue01ToInt(terrainHeight, 0, ChunkStats.height);
            return surfaceHeight;
        }
    }
}