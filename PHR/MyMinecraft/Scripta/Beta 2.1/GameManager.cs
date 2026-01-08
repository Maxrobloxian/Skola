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
        Vector2Int chunkCenter = Chunk.GetChunkCenter(new(0, 0));
        if (Physics.Raycast(new Vector3(chunkCenter.x, ChunkStats.height + 1, chunkCenter.y), Vector3.down, out RaycastHit hit, ChunkStats.height * 1.2f))
        {
            Instantiate(player, .5f * player.transform.localScale.y * Vector3.up + hit.point, Quaternion.identity);
        }
    }
}