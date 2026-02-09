using UnityEngine;

public class ChunkData
{
    public readonly Vector2Int worldPosition, realPosition;

    readonly BlockType[,,] blocks = new BlockType[ChunkSettings.width, ChunkSettings.height + 1, ChunkSettings.width];

    public ChunkData(int x, int z)
    {
        worldPosition = new Vector2Int(x, z);
        realPosition = new Vector2Int(x * 16, z * 16);
    }

    internal BlockType GetBlock(int x, int y, int z)
    {
        return blocks[x, y, z];
    }
    internal BlockType GetBlock(Vector3Int pos)
    {
        return blocks[pos.x, pos.y, pos.z];
    }

    internal void SetBlock(int x, int y, int z, BlockType blockType)
    {
        blocks[x, y, z] = blockType;
    }

    internal void SetBlock(Vector3Int pos, BlockType blockType)
    {
        SetBlock(pos.x, pos.y, pos.z, blockType);
    }
}