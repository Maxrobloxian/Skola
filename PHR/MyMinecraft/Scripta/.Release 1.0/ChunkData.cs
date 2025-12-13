using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class ChunkData : MonoBehaviour
{
    MeshFilter meshFilter;
    MeshCollider meshCollider;
    Mesh mesh;

    MeshData meshData = new(true);

    public BlockType[,,] blocks { get; private set; } = new BlockType[ChunkStats.width, ChunkStats.height+1, ChunkStats.width];

    public Vector2Int worldPosition { get; private set; }

    void Awake()
    {
        worldPosition = new Vector2Int((int)transform.position.x / 16, (int)transform.position.z / 16);

        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();
        mesh = meshFilter.mesh;
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32; //  Del Later ////////////////////////////////////////////////////////////////
    }

    internal void SetBlock(Vector3Int blockPosition, BlockType type)
    {
        blocks[blockPosition.x, blockPosition.y, blockPosition.z] = type;
    }

    public void SpawnChunk(bool firstSpawn = false)
    {
        ClearData();

        for (int x = 0; x < ChunkStats.width; x++)
        {
            for (int z = 0; z < ChunkStats.width; z++)
            {
                if (x != 0 && x != ChunkStats.width - 1 && z != 0 && z != ChunkStats.width - 1)
                {
                    SetEdgeBlockSides(x, 0, z);
                    for (int y = 1; y < ChunkStats.height; y++)
                    {
                        SetBlockSides(x, y, z);
                    }
                }
                else
                {
                    for (int y = 0; y < ChunkStats.height; y++)
                    {
                        SetEdgeBlockSides(x, y, z);
                    }
                }
            }
        }
        RenderMesh();
        if (firstSpawn)
        {
            UpdateNeighbours();
        }
    }

    void UpdateNeighbours()
    {
        try
        {
            ChunkDictionary.chunk[worldPosition + DirectionVectors.XZ[Direction.forward]].UpdateSide(Direction.backward);
        } catch { }
        try
        {
            ChunkDictionary.chunk[worldPosition + DirectionVectors.XZ[Direction.right]].UpdateSide(Direction.left);
        } catch { }
        try
        {
            ChunkDictionary.chunk[worldPosition + DirectionVectors.XZ[Direction.backward]].UpdateSide(Direction.forward);
        } catch { }
        try
        {
            ChunkDictionary.chunk[worldPosition + DirectionVectors.XZ[Direction.left]].UpdateSide(Direction.right);
        } catch { }
    }

    public void UpdateSide(Direction direction)
    {
        //int xWidth = direction.Equals(Direction.left) ? 1 : ChunkStats.width;
        //int zWidth = direction.Equals(Direction.backward) ? 1 : ChunkStats.width;

        //for (int x = DirectionVectors.chunkDirectionVector2[direction][0]; x < xWidth; x++)
        //{
        //    for (int z = DirectionVectors.chunkDirectionVector2[direction][1]; z < zWidth; z++)
        //    {
        //        for (int y = 0; y < ChunkStats.height; y++)
        //        {
        //            //SetEdgeBlockSides(x, y, z);
        //        }
        //    }
        //}
        SpawnChunk();
    }

    void RenderMesh()
    {
        mesh.Clear();

        mesh.subMeshCount = 2;
        mesh.vertices = meshData.vertices.Concat(meshData.waterMesh.vertices).ToArray();
        
        mesh.SetTriangles(meshData.triangles.ToArray(), 0);
        mesh.SetTriangles(meshData.waterMesh.triangles.Select(val => val + meshData.vertices.Count).ToArray(), 1);
        
        mesh.uv = meshData.uvs.Concat(meshData.waterMesh.uvs).ToArray();
        mesh.RecalculateNormals();

        Mesh collisionMesh = new()
        {
            vertices = meshData.colliderVertices.ToArray(),
            triangles = meshData.colliderTriangles.ToArray()
        };
        collisionMesh.RecalculateNormals();
        meshCollider.sharedMesh = collisionMesh;
    }

    void ClearData()
    {
        meshData.vertices.Clear();
        meshData.triangles.Clear();
        meshData.uvs.Clear();

        meshData.waterMesh.vertices.Clear();
        meshData.waterMesh.triangles.Clear();
        meshData.waterMesh.uvs.Clear();

        meshData.colliderVertices.Clear();
        meshData.colliderTriangles.Clear();

        meshData.waterMesh.colliderVertices.Clear();
        meshData.waterMesh.colliderTriangles.Clear();
    }

    void SetBlockSides(int x, int y, int z)
    {
        if (BlockDataManager.blockTexture[blocks[x, y, z]].isSolid)
        {
            for (int i = 0; i < 6; i++)
            {
                Vector3Int dirVectors = DirectionVectors.vector3[(Direction)i];
                if (!BlockDataManager.blockTexture[blocks[x + dirVectors.x, y + dirVectors.y, z + dirVectors.z]].isSolid)
                {
                    SetBlockSide((Direction)i, x, y, z);
                }
            }
        }
        else if (blocks[x, y, z] == BlockType.Water)
        {
            for (int i = 0; i < 6; i++)
            {
                Vector3Int dirVectors = DirectionVectors.vector3[(Direction)i];
                if (!BlockDataManager.blockTexture[blocks[x + dirVectors.x, y + dirVectors.y, z + dirVectors.z]].isSolid && blocks[x + dirVectors.x, y + dirVectors.y, z + dirVectors.z] != BlockType.Water)
                {
                    SetBlockSide((Direction)i, x, y, z);
                }
            }
        }
    }

    void SetEdgeBlockSides(int x, int y, int z)
    {
        if (BlockDataManager.blockTexture[blocks[x, y, z]].isSolid)
        {
            if (z != ChunkStats.width - 1)
            {
                if (!BlockDataManager.blockTexture[blocks[x, y, z + 1]].isSolid)
                {
                    SetBlockSide(Direction.forward, x, y, z);
                }
            }
            else
            {
                try
                {
                    if (!BlockDataManager.blockTexture[ChunkDictionary.chunk[worldPosition + Vector2Int.up].blocks[x, y, 0]].isSolid)
                    {
                        SetBlockSide(Direction.forward, x, y, z);
                    }
                }
                catch { }
            }
            if (x != ChunkStats.width - 1)
            {
                if (!BlockDataManager.blockTexture[blocks[x + 1, y, z]].isSolid)
                {
                    SetBlockSide(Direction.right, x, y, z);
                }
            }
            else
            {
                try
                {
                    if (!BlockDataManager.blockTexture[ChunkDictionary.chunk[worldPosition + Vector2Int.right].blocks[0, y, z]].isSolid)
                    {
                        SetBlockSide(Direction.right, x, y, z);
                    }
                }
                catch { }
            }
            if (z != 0)
            {
                if (!BlockDataManager.blockTexture[blocks[x, y, z - 1]].isSolid)
                {
                    SetBlockSide(Direction.backward, x, y, z);
                }
            }
            else
            {
                try
                {
                    if (!BlockDataManager.blockTexture[ChunkDictionary.chunk[worldPosition + Vector2Int.down].blocks[x, y, ChunkStats.width - 1]].isSolid)
                    {
                        SetBlockSide(Direction.backward, x, y, z);
                    }
                }
                catch { }
            }
            if (x != 0)
            {
                if (!BlockDataManager.blockTexture[blocks[x - 1, y, z]].isSolid)
                {
                    SetBlockSide(Direction.left, x, y, z);
                }
            }
            else
            {
                try
                {
                    if (!BlockDataManager.blockTexture[ChunkDictionary.chunk[worldPosition + Vector2Int.left].blocks[ChunkStats.width - 1, y, z]].isSolid)
                    {
                        SetBlockSide(Direction.left, x, y, z);
                    }
                }
                catch { }
            }
            if (!BlockDataManager.blockTexture[blocks[x, y + 1, z]].isSolid)
            {
                SetBlockSide(Direction.up, x, y, z);
            }
            if (y != 0 && !BlockDataManager.blockTexture[blocks[x, y - 1, z]].isSolid)
            {
                SetBlockSide(Direction.down, x, y, z);
            }
        }
        else
        {
            SetWaterEdgeBlockSides(x, y, z);
        }
    }

    void SetWaterEdgeBlockSides(int x, int y, int z)
    {
        if (blocks[x, y, z] == BlockType.Water)
        {
            if (z != ChunkStats.width - 1)
            {
                if (!BlockDataManager.blockTexture[blocks[x, y, z + 1]].isSolid && blocks[x, y, z + 1] != BlockType.Water)
                {
                    SetBlockSide(Direction.forward, x, y, z);
                }
            }
            else
            {
                try
                {
                    if (!BlockDataManager.blockTexture[ChunkDictionary.chunk[worldPosition + Vector2Int.up].blocks[x, y, 0]].isSolid && ChunkDictionary.chunk[worldPosition + Vector2Int.up].blocks[x, y, 0] != BlockType.Water)
                    {
                        SetBlockSide(Direction.forward, x, y, z);
                    }
                }
                catch { }
            }
            if (x != ChunkStats.width - 1)
            {
                if (!BlockDataManager.blockTexture[blocks[x + 1, y, z]].isSolid && blocks[x + 1, y, z] != BlockType.Water)
                {
                    SetBlockSide(Direction.right, x, y, z);
                }
            }
            else
            {
                try
                {
                    if (!BlockDataManager.blockTexture[ChunkDictionary.chunk[worldPosition + Vector2Int.right].blocks[0, y, z]].isSolid && ChunkDictionary.chunk[worldPosition + Vector2Int.right].blocks[0, y, z] != BlockType.Water)
                    {
                        SetBlockSide(Direction.right, x, y, z);
                    }
                }
                catch { }
            }
            if (z != 0)
            {
                if (!BlockDataManager.blockTexture[blocks[x, y, z - 1]].isSolid && blocks[x, y, z - 1] != BlockType.Water)
                {
                    SetBlockSide(Direction.backward, x, y, z);
                }
            }
            else
            {
                try
                {
                    if (!BlockDataManager.blockTexture[ChunkDictionary.chunk[worldPosition + Vector2Int.down].blocks[x, y, ChunkStats.width - 1]].isSolid && ChunkDictionary.chunk[worldPosition + Vector2Int.down].blocks[x, y, ChunkStats.width - 1] != BlockType.Water)
                    {
                        SetBlockSide(Direction.backward, x, y, z);
                    }
                }
                catch { }
            }
            if (x != 0)
            {
                if (!BlockDataManager.blockTexture[blocks[x - 1, y, z]].isSolid && blocks[x - 1, y, z] != BlockType.Water)
                {
                    SetBlockSide(Direction.left, x, y, z);
                }
            }
            else
            {
                try
                {
                    if (!BlockDataManager.blockTexture[ChunkDictionary.chunk[worldPosition + Vector2Int.left].blocks[ChunkStats.width - 1, y, z]].isSolid && ChunkDictionary.chunk[worldPosition + Vector2Int.left].blocks[ChunkStats.width - 1, y, z] != BlockType.Water)
                    {
                        SetBlockSide(Direction.left, x, y, z);
                    }
                }
                catch { }
            }
            if (!BlockDataManager.blockTexture[blocks[x, y + 1, z]].isSolid && blocks[x, y + 1, z] != BlockType.Water)
            {
                SetBlockSide(Direction.up, x, y, z);
            }
            if (y != 0 && !BlockDataManager.blockTexture[blocks[x, y - 1, z]].isSolid && blocks[x, y - 1, z] != BlockType.Water)
            {
                SetBlockSide(Direction.down, x, y, z);
            }
        }
    }

    void SetBlockSide(Direction direction, int x, int y, int z)
    {
        if (blocks[x, y, z] != BlockType.Water)
        {
            BlockHelper.GetFaceVertices(direction, x, y, z, meshData, blocks[x, y, z]);
            meshData.AddQuadTriangles(BlockDataManager.blockTexture[blocks[x, y, z]].generatesCollider);
            meshData.uvs.AddRange(BlockHelper.FaceUVs(direction, blocks[x, y, z]));
        }
        else
        {
            BlockHelper.GetFaceVertices(direction, x, y, z, meshData.waterMesh, blocks[x, y, z]);
            meshData.waterMesh.AddQuadTriangles(BlockDataManager.blockTexture[blocks[x, y, z]].generatesCollider);
            meshData.waterMesh.uvs.AddRange(BlockHelper.FaceUVs(direction, blocks[x, y, z]));
        }
    }
}