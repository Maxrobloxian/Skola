using System.Collections.Generic;
using UnityEngine;

public class MeshData
{
    public List<Vector3> vertices = new();
    public List<int> triangles = new();
    public List<Vector2> uvs = new();

    public List<Vector3> colliderVertices = new();
    public List<int> colliderTriangles = new();

    public MeshData waterMesh;

    public MeshData(bool isWaterMesh)
    {
        if (isWaterMesh)
        {
            waterMesh = new(false);
        }
    }

    public void AddVertex(Vector3 vertex, bool vertexGeneratesCollider)
    {
        vertices.Add(vertex);
        if (vertexGeneratesCollider)
        {
            colliderVertices.Add(vertex);
        }

    }

    public void AddQuadTriangles(bool quadGeneratesCollider)
    {
        triangles.Add(vertices.Count - 4);
        triangles.Add(vertices.Count - 3);
        triangles.Add(vertices.Count - 2);

        triangles.Add(vertices.Count - 4);
        triangles.Add(vertices.Count - 2);
        triangles.Add(vertices.Count - 1);

        if (quadGeneratesCollider)
        {
            colliderTriangles.Add(colliderVertices.Count - 4);
            colliderTriangles.Add(colliderVertices.Count - 3);
            colliderTriangles.Add(colliderVertices.Count - 2);
            colliderTriangles.Add(colliderVertices.Count - 4);
            colliderTriangles.Add(colliderVertices.Count - 2);
            colliderTriangles.Add(colliderVertices.Count - 1);
        }
    }
}