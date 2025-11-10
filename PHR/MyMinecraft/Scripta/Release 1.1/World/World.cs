using UnityEngine;

public class World : MonoBehaviour
{
    [SerializeField] int worldSize = 3;
    [SerializeField] GameObject chunk;

    private void Start()
    {
        WorldGenerate.GenWorld(worldSize, this);
    }

    internal ChunkRenderer InstantiateChunk(ChunkData chunkData)
    {
        return Instantiate(chunk, new(chunkData.realPosition.x, 0, chunkData.realPosition.y), Quaternion.identity, transform).GetComponent<ChunkRenderer>();
    }
}