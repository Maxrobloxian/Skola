using System.Collections.Generic;
using UnityEngine;

public class WorldRenderer : MonoBehaviour
{
    [SerializeField] GameObject chunk;
    readonly Queue<ChunkRenderer> chunkPool = new();

    internal ChunkRenderer RenderChunk(ChunkData chunkData, MeshData meshData)
    {
        ChunkRenderer newChunk;
        if (chunkPool.Count > 0)
        {
            newChunk = chunkPool.Dequeue();
            newChunk.transform.position = new(chunkData.realPosition.x, 0, chunkData.realPosition.y);
        }
        else
        {
            newChunk = Instantiate(chunk, new(chunkData.realPosition.x, 0, chunkData.realPosition.y), Quaternion.identity, transform).GetComponent<ChunkRenderer>();
            //print("new chunk created");
        }

        newChunk.SetData(chunkData);
        newChunk.RenderChunk(meshData);
        newChunk.gameObject.SetActive(true);

        return newChunk;
    }

    internal void RemoveChunk(ChunkRenderer chunkRenderer)
    {
        chunkRenderer.gameObject.SetActive(false);
        chunkPool.Enqueue(chunkRenderer);
    }
}