using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    [SerializeField] UIManager uiManager;
    Transform player;

#if DEBUG
    public bool enablePerformanceStats = true;

    private void Awake()
    {
        if (enablePerformanceStats) gameObject.AddComponent<PerformanceStats>();

        if (GetComponent<DebugChunkBorders>().enabled) print("Turn DebugChunkBorders off!!!");
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

        // Add player inventory to global settings
        GetComponent<GlobalSettingsInitializer>().PlayerSettingsAddData(player.GetComponent<PlayerInventory>());
        
        // Enable hotbar UI
        uiManager.ToggleHotbar(true);
    }

    public Transform GetPlayerTransform()
    {
        return player.transform;
    }
}