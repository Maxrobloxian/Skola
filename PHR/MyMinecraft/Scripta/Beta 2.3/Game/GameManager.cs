using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    [SerializeField] UIManager uiManager;
    Transform player;

    [SerializeField] UnityEvent playerSpawnEvent;

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
        if (Physics.Raycast(new Vector3(chunkCenter.x, ChunkSettings.height + 1, chunkCenter.y), Vector3.down, out RaycastHit hit, ChunkSettings.height * 1.2f))
        {
            player = Instantiate(playerPrefab, .5f * playerPrefab.transform.localScale.y * Vector3.up + hit.point, Quaternion.identity).transform;
        }

        GetComponent<GlobalSettingsInitializer>().PlayerSettingsAddData(player);

        playerSpawnEvent.Invoke();
    }

    public Transform GetPlayerTransform()
    {
        return player.transform;
    }
}