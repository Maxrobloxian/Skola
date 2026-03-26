using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    [SerializeField] UIManager uiManager;
    Transform player;

    [SerializeField] UnityEvent playerSpawnEvent;

    public void SpawnPlayer(Vector3 playerRealPos)
    {
        if (playerRealPos != Vector3.zero)
        {
            player = Instantiate(playerPrefab, playerRealPos, Quaternion.identity).transform;
        }
        else
        {
            Vector2Int chunkCenter = Chunk.GetChunkCenter(Vector2Int.zero);
            if (Physics.Raycast(new Vector3(chunkCenter.x, ChunkSettings.height + 1, chunkCenter.y), Vector3.down, out RaycastHit hit, ChunkSettings.height * 1.2f))
            {
                player = Instantiate(playerPrefab, .5f * playerPrefab.transform.localScale.y * Vector3.up + hit.point, Quaternion.identity).transform;
            }
        }

        GetComponent<GlobalSettingsInitializer>().PlayerSettingsAddData(player);

        playerSpawnEvent.Invoke();
    }

    // In editor
    public void TogglePause(bool pause)
    {
        Time.timeScale = pause ? 0 : 1;
    }

    public void QuitToTitle()
    {
        CloseGame();

        SceneManager.LoadSceneAsync(0);
    }
    // ----

    void OnApplicationQuit()
    {
        CloseGame();
    }

    void SaveGame()
    {
        ChunkFileHandler.Save();
        InventoryFileHandler.Save();
        PlayerFileHandler.Save();
    }

    void DeleteGameData()
    {
        ItemIconData.DeleteIcons();
        WorldData.DeleteData();
    }

    void CloseGame()
    {
        SaveGame();

        DeleteGameData();

        Time.timeScale = 1;
    }
}