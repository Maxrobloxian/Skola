using System;
using System.Collections.Generic;
using UnityEngine;

public static class Chunk
{
    public static void LoopThroughBlocks(Action<int, int, int> actionToPerform)
    {
        for (int x = 0; x < ChunkSettings.width; x++)
        {
            for (int y = 0; y < ChunkSettings.height; y++)
            {
                for (int z = 0; z < ChunkSettings.width; z++)
                {
                    actionToPerform(x, y, z);
                }
            }
        }
    }

    internal static MeshData GetChunkMeshData(ChunkData chunkData)
    {
        MeshData meshData = new(true);

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

    internal static (ChunkRenderer, Vector3Int) GetOutOfBoundChunkRenderer(ChunkRenderer chunkRenderer, Vector3Int blockPos)
    {
        if (InRangeWidth(blockPos.x) && InRangeWidth(blockPos.z))
        {
            return (chunkRenderer, blockPos);
        }

        return GetChunkRendererFromNeighborChunk(chunkRenderer.chunkData.realPosition.x + blockPos.x, blockPos.y,chunkRenderer.chunkData.realPosition.y + blockPos.z);
    }
    static (ChunkRenderer, Vector3Int) GetChunkRendererFromNeighborChunk(int realPosPlsuBlockX, int y, int realPosPlsuBlockZ)
    {
        WorldData.chunkRenderer.TryGetValue(RealPosPlusBlockToWorldPos(realPosPlsuBlockX, realPosPlsuBlockZ), out ChunkRenderer chunkRenderer);

        return GetOutOfBoundChunkRenderer(chunkRenderer, new(realPosPlsuBlockX - chunkRenderer.chunkData.realPosition.x, y, realPosPlsuBlockZ - chunkRenderer.chunkData.realPosition.y));
    }

    internal static (ChunkData, Vector3Int) GetOutOfBoundChunkData(ChunkData chunkData, Vector3Int blockPos)
    {
        if (InRangeWidth(blockPos.x) && InRangeWidth(blockPos.z))
        {
            return (chunkData, blockPos);
        }

        return GetChunkDataFromNeighborChunk(chunkData.realPosition.x + blockPos.x, blockPos.y, chunkData.realPosition.y + blockPos.z);
    }
    static (ChunkData, Vector3Int) GetChunkDataFromNeighborChunk(int realPosPlsuBlockX, int y, int realPosPlsuBlockZ)
    {
        WorldData.chunkData.TryGetValue(RealPosPlusBlockToWorldPos(realPosPlsuBlockX, realPosPlsuBlockZ), out ChunkData chunkData);

        return GetOutOfBoundChunkData(chunkData, new(realPosPlsuBlockX - chunkData.realPosition.x, y, realPosPlsuBlockZ - chunkData.realPosition.y));
    }

    internal static Vector2Int RealPosPlusBlockToWorldPos(int x, int z)
    {
        return new Vector2Int()
        {
            x = Mathf.FloorToInt(x / (float)ChunkSettings.width),
            y = Mathf.FloorToInt(z / (float)ChunkSettings.width)
        };
    }

    static bool InRangeWidth(int value)
    {
        if (value < 0 || value >= ChunkSettings.width)
            return false;

        return true;
    }
    static bool InRangeHeight(int value)
    {
        if (value < 0 || value >= ChunkSettings.height)
            return false;

        return true;
    }

    public static Vector2Int GetChunkCenter(Vector2Int chunkWorldPosition)
    {
        return new Vector2Int()
        {
            x = chunkWorldPosition.x * ChunkSettings.width + ChunkSettings.width / 2,
            y = chunkWorldPosition.y * ChunkSettings.width + ChunkSettings.width / 2
        };
    }

    public static Vector2Int GetChunkWorldPosition(Vector2Int realPosition)
    {
        return new Vector2Int()
        {
            x = Mathf.FloorToInt(realPosition.x / (float)ChunkSettings.width),
            y = Mathf.FloorToInt(realPosition.y / (float)ChunkSettings.width)
        };
    }

    public static Vector2Int GetChunkWorldPosition(Vector2 realPosition)
    {
        return GetChunkWorldPosition(new Vector2Int(Mathf.FloorToInt(realPosition.x), Mathf.FloorToInt(realPosition.y)));
    }

    public static Vector3Int GetRaycastedBlockPosition(RaycastHit hit, Vector2Int chunkDataRealPosition)
    {
        hit.normal *= .5f;
        return Vector3Int.FloorToInt(new(hit.point.x - chunkDataRealPosition.x - hit.normal.x, hit.point.y - hit.normal.y, hit.point.z - chunkDataRealPosition.y - hit.normal.z));
    }

    public static Vector3Int GetRaycastedBlockPositionOnSide(RaycastHit hit, Vector2Int chunkDataRealPosition)
    {
        hit.normal *= .5f;
        return Vector3Int.FloorToInt(new(hit.point.x - chunkDataRealPosition.x + hit.normal.x, hit.point.y + hit.normal.y, hit.point.z - chunkDataRealPosition.y + hit.normal.z));
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
        if (x == 0 || z == 0 || x == ChunkSettings.width - 1 || z == ChunkSettings.width - 1) return true;
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
        else if (blockPos.x == ChunkSettings.width - 1) worldPositions.Add(worldPos + Vector2Int.right);

        if (blockPos.y == 0) worldPositions.Add(worldPos + Vector2Int.down);
        else if (blockPos.y == ChunkSettings.width - 1) worldPositions.Add(worldPos + Vector2Int.up);
        
        return worldPositions;
    }
    public static List<Vector2Int> GetEdgeNeighboursWorldPos(Vector2Int worldPos, Vector3Int blockPos)
    {
        return GetEdgeNeighboursWorldPos(worldPos, new Vector2Int(blockPos.x, blockPos.z));
    }

    public static void UpdateChunksNearBlock(Vector3Int blockPos, ChunkRenderer chunkRenderer)
    {
        UpdateChunksNearBlock(blockPos, chunkRenderer.chunkData);
    }
    public static void UpdateChunksNearBlock(Vector3Int blockPos, ChunkData chunkData)
    {
        if (IsBlockOnEdge(blockPos))
        {
            foreach (Vector2Int neighbourWorldPos in GetEdgeNeighboursWorldPos(chunkData.worldPosition, blockPos))
            {
                ChunkRenderer neighbourChunkRenderer = WorldData.chunkRenderer[neighbourWorldPos];
                neighbourChunkRenderer.RenderChunk(GetChunkMeshData(neighbourChunkRenderer.chunkData));
            }
        }
        WorldData.chunkRenderer[chunkData.worldPosition].RenderChunk(GetChunkMeshData(chunkData));
    }
}