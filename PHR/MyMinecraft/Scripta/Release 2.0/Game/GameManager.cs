using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject player;

#if DEBUG
    public bool enablePerformanceStats = true;

    private void Awake()
    {
        if (enablePerformanceStats) gameObject.AddComponent<PerformanceStats>();
    }

    public void DestroyPerformanceStats()
    {
        Destroy(GetComponent<PerformanceStats>());
    }
#endif

    public void SpawnPlayer()
    {
        //Vector2Int chunkCenter = Chunk.GetChunkCenter(new(0, 0));
        //if (Physics.Raycast(new Vector3(chunkCenter.x, ChunkStats.height, chunkCenter.y), Vector3.down, out RaycastHit hit, ChunkStats.height * 1.2f))
        //{
        //    Instantiate(player, Vector3.up * player.transform.localScale.y + hit.point, Quaternion.identity);
        //}

        Vector2Int chunkCenter = Chunk.GetChunkCenter(new(0, 0));
        Instantiate(player, new Vector3(chunkCenter.x, 100, chunkCenter.y), Quaternion.identity);
    }
}