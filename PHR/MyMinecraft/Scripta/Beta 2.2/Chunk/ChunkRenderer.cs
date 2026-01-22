using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class ChunkRenderer : MonoBehaviour
{
    MeshFilter meshFilter;
    MeshCollider meshCollider;
    Mesh mesh;

    public ChunkData chunkData { get; private set; }

    void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();
        mesh = meshFilter.mesh;
    }

    internal void SetData(ChunkData chunkData)
    {
        this.chunkData = chunkData;
    }

    internal void RenderChunk(MeshData meshData)
    {
        RenderMesh(meshData);
    }

    void RenderMesh(MeshData meshData)
    {
        mesh.Clear();

        mesh.subMeshCount = 2;
        mesh.vertices = meshData.vertices.Concat(meshData.waterMesh.vertices).ToArray();

        mesh.SetTriangles(meshData.triangles.ToArray(), 0);
        mesh.SetTriangles(meshData.waterMesh.triangles.Select(val => val + meshData.vertices.Count).ToArray(), 1);

        mesh.uv = meshData.uvs.Concat(meshData.waterMesh.uvs).ToArray();
        mesh.RecalculateNormals();

        meshCollider.sharedMesh = null;
        Mesh collisionMesh = new()
        {
            vertices = meshData.colliderVertices.ToArray(),
            triangles = meshData.colliderTriangles.ToArray()
        };
        collisionMesh.RecalculateNormals();
        meshCollider.sharedMesh = collisionMesh;
    }
}