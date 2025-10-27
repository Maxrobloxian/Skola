using UnityEngine;

public static class BlockHelper
{
    public static void GetFaceVertices(Direction direction, int x, int y, int z, MeshData meshData, BlockType blockType)
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
        }
    }

    public static Vector2Int TexturePosition(BlockType blockType, Direction direction)
    {
        return direction switch
        {
            Direction.up => BlockDataManager.blockTexture[blockType].up,
            Direction.down => BlockDataManager.blockTexture[blockType].down,
            _ => BlockDataManager.blockTexture[blockType].side
        };
    }

    public static Vector2[] FaceUVs(Direction direction, BlockType blockType)
    {
        Vector2[] UVs = new Vector2[4];
        Vector2Int tilePos = TexturePosition(blockType, direction);

        UVs[0] = new Vector2(BlockDataManager.tileSize * tilePos.x + BlockDataManager.tileSize - BlockDataManager.textureOffset, BlockDataManager.tileSize * tilePos.y + BlockDataManager.textureOffset);
        UVs[1] = new Vector2(BlockDataManager.tileSize * tilePos.x + BlockDataManager.tileSize - BlockDataManager.textureOffset, BlockDataManager.tileSize * tilePos.y + BlockDataManager.tileSize - BlockDataManager.textureOffset);
        UVs[2] = new Vector2(BlockDataManager.tileSize * tilePos.x + BlockDataManager.textureOffset, BlockDataManager.tileSize * tilePos.y + BlockDataManager.tileSize - BlockDataManager.textureOffset);
        UVs[3] = new Vector2(BlockDataManager.tileSize * tilePos.x + BlockDataManager.textureOffset, BlockDataManager.tileSize * tilePos.y + BlockDataManager.textureOffset);

        //UVs[0] = new Vector2(tilePos + 0.001f, 0.001f);
        //UVs[1] = new Vector2(tilePos + 0.001f, 0.999f);
        //UVs[2] = new Vector2(tilePos + 0.1656667f, 0.999f);
        //UVs[3] = new Vector2(tilePos + 0.1656667f, 0.001f);

        return UVs;
    }
}