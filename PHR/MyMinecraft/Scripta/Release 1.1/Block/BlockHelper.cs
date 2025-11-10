using UnityEngine;

public static class BlockHelper
{
    internal static void SetBlockSides(ChunkData chunkData, int x, int y, int z, MeshData meshData, BlockType blockType)
    {
        if (blockType == BlockType.Air || blockType == BlockType.Nothing) return;

        for (int i = 0; i < 6; i++)
        {
            BlockType neighbourBlockType = Chunk.GetBlockFromChunk(chunkData, new Vector3Int(x, y, z) + DirectionExtensions.directions[i].GetVector());

            if (neighbourBlockType != BlockType.Nothing && BlockDataManager.blockTexture[neighbourBlockType].isSolid == false)
            {
                if (blockType == BlockType.Water)
                {
                    if (neighbourBlockType == BlockType.Air)
                        SetBlockSide(DirectionExtensions.directions[i], x, y, z, meshData.waterMesh, blockType);
                }
                else
                {
                    SetBlockSide(DirectionExtensions.directions[i], x, y, z, meshData, blockType);
                }
            }
        }
    }

    static void SetBlockSide(Direction direction, int x, int y, int z, MeshData meshData, BlockType blockType)
    {
        SetFaceVertices(direction, x, y, z, meshData, blockType);
        meshData.AddQuadTriangles(BlockDataManager.blockTexture[blockType].generatesCollider);
        meshData.uvs.AddRange(FaceUVs(direction, blockType));
    }

    static void SetFaceVertices(Direction direction, int x, int y, int z, MeshData meshData, BlockType blockType)
    {
        bool isCollider = BlockDataManager.blockTexture[blockType].generatesCollider;
        
        switch (direction)
        {
            case Direction.backward:
                meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f), isCollider);
                meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f), isCollider);
                meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f), isCollider);
                meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f), isCollider);
                break;
            case Direction.forward:
                meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f), isCollider);
                meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f), isCollider);
                meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f), isCollider);
                meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f), isCollider);
                break;
            case Direction.left:
                meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f), isCollider);
                meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f), isCollider);
                meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f), isCollider);
                meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f), isCollider);
                break;

            case Direction.right:
                meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f), isCollider);
                meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f), isCollider);
                meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f), isCollider);
                meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f), isCollider);
                break;
            case Direction.down:
                meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f), isCollider);
                meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f), isCollider);
                meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f), isCollider);
                meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f), isCollider);
                break;
            case Direction.up:
                meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f), isCollider);
                meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f), isCollider);
                meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f), isCollider);
                meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f), isCollider);
                break;
            default:
                break;
        }
    }

    static Vector2[] FaceUVs(Direction direction, BlockType blockType)
    {
        Vector2[] UVs = new Vector2[4];
        Vector2Int tilePos = GetTexturePosition(direction, blockType);

        UVs[0] = new Vector2(BlockDataManager.tileSize * tilePos.x + BlockDataManager.tileSize - BlockDataManager.textureOffset, BlockDataManager.tileSize * tilePos.y + BlockDataManager.textureOffset);
        UVs[1] = new Vector2(BlockDataManager.tileSize * tilePos.x + BlockDataManager.tileSize - BlockDataManager.textureOffset,BlockDataManager.tileSize * tilePos.y + BlockDataManager.tileSize - BlockDataManager.textureOffset);
        UVs[2] = new Vector2(BlockDataManager.tileSize * tilePos.x + BlockDataManager.textureOffset,BlockDataManager.tileSize * tilePos.y + BlockDataManager.tileSize - BlockDataManager.textureOffset);
        UVs[3] = new Vector2(BlockDataManager.tileSize * tilePos.x + BlockDataManager.textureOffset,BlockDataManager.tileSize * tilePos.y + BlockDataManager.textureOffset);

        return UVs;
    }

    public static Vector2Int GetTexturePosition(Direction direction, BlockType blockType)
    {
        return direction switch
        {
            Direction.up => BlockDataManager.blockTexture[blockType].up,
            Direction.down => BlockDataManager.blockTexture[blockType].down,
            _ => BlockDataManager.blockTexture[blockType].side
        };
    }
}