using System;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
    public static void LoopThroughBlocks(Action<int, int, int> actionToPerform)
    {
        for (int x = 0; x < ChunkStats.width; x++)
        {
            for (int y = 0; y < ChunkStats.height; y++)
            {
                for (int z = 0; z < ChunkStats.width; z++)
                {
                    actionToPerform(x, y, z);
                }
            }
        }
    }

    internal static MeshData GetChunkMeshData(ChunkData chunkData)
    {
        MeshData meshData = new (true);

        LoopThroughBlocks((x, y, z) => BlockHelper.SetBlockSides(chunkData, x, y, z, meshData, chunkData.GetBlock(x, y, z)));

        return meshData;
    }

    internal static BlockType GetBlockFromChunk(ChunkData chunkData, Vector3Int blockPos)
    {
        return GetBlockFromChunk(chunkData, blockPos.x, blockPos.y, blockPos.z);
    }
    internal static BlockType GetBlockFromChunk(ChunkData chunkData, int x, int y, int z)
    {
        if (InRangeWidth(x) && InRangeWidth(z))
        {
            if (InRangeHeight(y))
            {
                return chunkData.GetBlock(x, y, z);
            }
            return BlockType.Air;
        }

        return GetBlockFromNeighborChunk(chunkData.realPosition.x + x, y, chunkData.realPosition.y + z);
    }
    internal static BlockType GetBlockFromNeighborChunk(int realPosPlsuBlockX, int y, int realPosPlsuBlockZ)
    {
        WorldData.chunkData.TryGetValue(RealPosPlusBlockToWorldPos(realPosPlsuBlockX, realPosPlsuBlockZ), out ChunkData chunkData);

        if (chunkData == null)
            return BlockType.Nothing;

        return GetBlockFromChunk(chunkData, realPosPlsuBlockX - chunkData.realPosition.x, y, realPosPlsuBlockZ - chunkData.realPosition.y);
    }

    internal static Vector2Int RealPosPlusBlockToWorldPos(int x, int z)
    {
        return new Vector2Int()
        {
            x = Mathf.FloorToInt(x / (float)ChunkStats.width),
            y = Mathf.FloorToInt(z / (float)ChunkStats.width)
        };
    }

    static bool InRangeWidth(int value)
    {
        if (value < 0 || value >= ChunkStats.width)
            return false;

        return true;
    }
    static bool InRangeHeight(int value)
    {
        if (value < 0 || value >= ChunkStats.height)
            return false;

        return true;
    }

    public static Vector2Int GetChunkCenter(Vector2Int chunkPosition)
    {
        return new Vector2Int()
        {
            x = chunkPosition.x * ChunkStats.width + ChunkStats.width / 2,
            y = chunkPosition.y * ChunkStats.width + ChunkStats.width / 2
        };
    }

    public static Vector2Int GetChunkPosition(Vector2Int realPosition)
    {
        return new Vector2Int()
        {
            x = Mathf.FloorToInt(realPosition.x / (float)ChunkStats.width),
            y = Mathf.FloorToInt(realPosition.y / (float)ChunkStats.width)
        };
    }

    public static Vector2Int GetChunkPosition(Vector2 realPosition)
    {
        return GetChunkPosition(new Vector2Int(Mathf.FloorToInt(realPosition.x), Mathf.FloorToInt(realPosition.y)));
    }

    public static Vector3Int GetRaycastBlockPosition(RaycastHit hit, Vector2Int chunkDataRealPosition)
    {
        return Vector3Int.RoundToInt(new Vector3(GetBlockPositionIn(hit.point.x - chunkDataRealPosition.x, hit.normal.x), GetBlockPositionIn(hit.point.y, hit.normal.y), GetBlockPositionIn(hit.point.z - chunkDataRealPosition.y, hit.normal.z)));

        static float GetBlockPositionIn(float position, float normal)
        {
            if (Mathf.Abs(position % 1) == .5f)
            {
                return position - (normal * .5f);
            }
            return position;
        }
    }

    public static Vector3Int GetBlockPosInChunk(ChunkData chunkData, int realPositionX, int realPositionZ)
    {
        return new(realPositionX - chunkData.realPosition.x, realPositionZ - chunkData.realPosition.y);
    }
    public static Vector3Int GetBlockPosInChunk(ChunkData chunkData, Vector3Int realPosition)
    {
        return GetBlockPosInChunk(chunkData, realPosition.x, realPosition.z);
    }

    public static bool IsBlockOnEdge(int x, int z)
    {
        if (x == 0 || z == 0 || x == ChunkStats.width - 1 || z == ChunkStats.width - 1) return true;
        return false;
    }
    public static bool IsBlockOnEdge(Vector3Int pos)
    {
        return IsBlockOnEdge(pos.x, pos.z);
    }

    public static List<Vector2Int> GetEdgeNeighboursWorldPos(Vector2Int worldPos, Vector2Int blockPos)
    {
        List<Vector2Int> worldPositions = new();
        if (blockPos.x == 0) worldPositions.Add(worldPos + Vector2Int.left);
        if (blockPos.y == 0) worldPositions.Add(worldPos + Vector2Int.down);
        if (blockPos.x == ChunkStats.width - 1) worldPositions.Add(worldPos + Vector2Int.right);
        if (blockPos.y == ChunkStats.width - 1) worldPositions.Add(worldPos + Vector2Int.up);
        return worldPositions;
    }
    public static List<Vector2Int> GetEdgeNeighboursWorldPos(Vector2Int worldPos, Vector3Int blockPos)
    {
        return GetEdgeNeighboursWorldPos(worldPos, new Vector2Int(blockPos.x, blockPos.z));
    }
}