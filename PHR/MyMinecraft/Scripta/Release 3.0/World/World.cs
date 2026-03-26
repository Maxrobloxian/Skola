using System.Collections;
using UnityEngine;

public class World : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    WorldGenerate worldGenerate;

#if DEBUG
    [SerializeField] bool stopPlayerLoad;
#endif

    private void Start()
    {
        worldGenerate = GetComponent<WorldGenerate>();
        StartWorld();
    }

    async void StartWorld()
    {
        // Maybe var -> vector3 ---------------------------------------------------------------------------------
        var playerRealPos = PlayerFileHandler.LoadPosition() ?? Vector3.zero;
        Vector2Int playerWorldPos = new (Mathf.FloorToInt(playerRealPos.x / ChunkSettings.width), Mathf.FloorToInt(playerRealPos.z / ChunkSettings.width));

        Vector2Int playerChunkCenter = Chunk.GetChunkCenter(playerWorldPos);
        //Camera.main.transform.position = new Vector3(-161.411407f + playerChunkCenter.x, 127.89917f, -12.0436296f + playerChunkCenter.y);
        //Camera.main.transform.SetPositionAndRotation(new Vector3(-128 + playerChunkCenter.x, 81, -27 + playerChunkCenter.y), Quaternion.Euler(38, 77, 0));
        Camera.main.transform.SetPositionAndRotation(new Vector3(-110 + playerChunkCenter.x, 110, -27 + playerChunkCenter.y), Quaternion.Euler(52, 77, 0));

        await worldGenerate.GenWorld(playerWorldPos);

#if DEBUG
        if (stopPlayerLoad) return;
#endif
        StartCoroutine(WaitForWorldGen(playerRealPos));
    }

    // In editor
    public async void UpdateWorld()
    {
        await worldGenerate.GenWorld(PlayerSettings.playerCharacter.GetWorldPosXZ());
    }
    // ----

    internal GameManager GetGameManager()
    {
        return gameManager;
    }

    IEnumerator WaitForWorldGen(Vector3 playerRealPos)
    {
        while (transform.childCount < Mathf.Pow(OptionsSettings.realRenderDistance * 2 + 1, 2) && transform.childCount < 121) yield return new WaitForEndOfFrame();
        gameManager.SpawnPlayer(playerRealPos);
    }
}