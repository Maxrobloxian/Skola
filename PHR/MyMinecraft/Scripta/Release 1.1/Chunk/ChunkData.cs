using UnityEngine;

public class ChunkData
{
    public readonly Vector2Int worldPosition, realPosition;

    readonly BlockType[,,] blocks = new BlockType[ChunkStats.width, ChunkStats.height + 1, ChunkStats.width];

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

    internal void SetBlock(Vector3Int blockPosition, BlockType blockType)
    {
        blocks[blockPosition.x, blockPosition.y, blockPosition.z] = blockType;
    }
}