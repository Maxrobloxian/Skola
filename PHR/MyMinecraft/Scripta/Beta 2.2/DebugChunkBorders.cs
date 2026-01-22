using UnityEngine;

public class DebugChunkBorders : MonoBehaviour
{
    Material lineMaterial;
    PlayerChunksManager playerChunksManager;

    Color yellow = new(1f, 0.92f, 0.016f, .75f);
    Color red = new(1f, 0, 0, .75f);

    private void Start()
    {
        playerChunksManager = GetComponent<GameManager>().GetPlayerTransform().GetComponent<PlayerChunksManager>();

        if (!lineMaterial) lineMaterial = new Material(Shader.Find("Hidden/Internal-Colored"));
    }

    void OnRenderObject()
    {
        lineMaterial.SetPass(0);

        GL.PushMatrix();
        // Match the GL coordinate space with the world space
        GL.MultMatrix(transform.localToWorldMatrix);

        GL.Begin(GL.LINES);
        GL.Color(red);

        // Draw borders
        Vector2 playerChunk = playerChunksManager.GetWorldPosXZ() * 16;
        DrawChunkEdge(new(playerChunk.x - .5f, 0, playerChunk.y - .5f));
        DrawPlayerChunkBorder(new(playerChunk.x - .5f, 0, playerChunk.y - .5f));

        GL.End();
        GL.PopMatrix();
    }

    void DrawChunkEdge(Vector3 chunkPosition)
    {
        for (int x = -16; x < 48; x += 16)
        {
            for (int z = -16; z < 48; z += 16)
            {
                GL.Vertex(chunkPosition + new Vector3(x, 0, z)); GL.Vertex(chunkPosition + new Vector3(x, ChunkSettings.height, z));
            }
        }
    }

    void DrawPlayerChunkBorder(Vector3 chunkPosition)
    {
        GL.Color(yellow);

        Vector3 chunkSize = ChunkSettings.size;

        // Horizontal
        for (int i = 0; i <= ChunkSettings.height; i += 2)
        {
            GL.Vertex(chunkPosition + new Vector3(0, i, 0)); GL.Vertex(chunkPosition + new Vector3(chunkSize.x, i, 0));
            GL.Vertex(chunkPosition + new Vector3(chunkSize.x, i, 0)); GL.Vertex(chunkPosition + new Vector3(chunkSize.x, i, chunkSize.z));
            GL.Vertex(chunkPosition + new Vector3(chunkSize.x, i, chunkSize.z)); GL.Vertex(chunkPosition + new Vector3(0, i, chunkSize.z));
            GL.Vertex(chunkPosition + new Vector3(0, i, chunkSize.z)); GL.Vertex(chunkPosition + new Vector3(0, i, 0));
        }

        // Vertical
        for (int x = 0; x <= ChunkSettings.width; x += 2)
        {
            if (x == 0 || x == ChunkSettings.width) for (int z = 2; z < ChunkSettings.width; z += 2)
            {
                GL.Vertex(chunkPosition + new Vector3(x, 0, z)); GL.Vertex(chunkPosition + new Vector3(x, chunkSize.y, z));
            }
            else
            {
                GL.Vertex(chunkPosition + new Vector3(x, 0, 0)); GL.Vertex(chunkPosition + new Vector3(x, chunkSize.y, 0));
                GL.Vertex(chunkPosition + new Vector3(x, 0, chunkSize.z)); GL.Vertex(chunkPosition + new Vector3(x, chunkSize.y, chunkSize.z));
            }
        }

        GL.Color(red);
    }
}