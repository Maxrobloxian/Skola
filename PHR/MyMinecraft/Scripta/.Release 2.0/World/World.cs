using UnityEngine;

public class World : MonoBehaviour
{
    [SerializeField] GameObject chunk;
    [SerializeField] GameManager gameManager;

    private void Start()
    {
        WorldGenerate.GenWorld(Vector2Int.zero, this);
        gameManager.SpawnPlayer();
    }

    internal ChunkRenderer InstantiateChunk(ChunkData chunkData)
    {
        return Instantiate(chunk, new(chunkData.realPosition.x, 0, chunkData.realPosition.y), Quaternion.identity, transform).GetComponent<ChunkRenderer>();
    }

    internal void UpdateWorld(Vector2Int playerWorldPosition)
    {
        WorldGenerate.GenWorld(playerWorldPosition, this);
    }

    internal void DestroyChunk(GameObject gameObject)
    {
        Destroy(gameObject);
    }
}