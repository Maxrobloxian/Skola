using UnityEngine;
using UnityEngine.Profiling;

public class DebugScreen : MonoBehaviour
{
    [SerializeField] UIDebugScreen uiDebugScreen;

    PlayerChunksManager playerChunksManager;
    PlayerCharacter playerCharacter;
    PlayerInteractions playerInteractions;

    Vector2Int chunkPos;
    Vector3Int playerPos;

    private float _accumulatedTime = 0f;
    private int _frameCount = 0;

    bool first = true;
    private void OnEnable()
    {
        if (first) FirstBoot();

        uiDebugScreen.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        uiDebugScreen.gameObject.SetActive(false);
    }

    void Update()
    {
        chunkPos = playerChunksManager.oldChunk;
        playerPos = playerCharacter.GetPosToInt();

        uiDebugScreen.SetPlayerPos(playerPos);
        uiDebugScreen.SetInChunkPos(new(playerPos.x - chunkPos.x * ChunkSettings.width, playerPos.y, playerPos.z - chunkPos.y * ChunkSettings.width));
        uiDebugScreen.SetChunkPos(chunkPos);
        uiDebugScreen.SetDirection(playerCharacter.GetDirection());

        SetTargetBlock();
    }

    private void FixedUpdate()
    {
        SetFPS();
        SetMemory();
    }

    void FirstBoot()
    {
        playerChunksManager = PlayerSettings.playerChunksManager;
        playerCharacter = PlayerSettings.playerCharacter;
        playerInteractions = PlayerSettings.playerInteractions;

        chunkPos = playerChunksManager.oldChunk;
        playerPos = playerCharacter.GetPosToInt();

        uiDebugScreen.SetVersion(GameSettings.versionType, (GameSettings.version.Build == 0) ? $"{GameSettings.version.Major}.{GameSettings.version.Minor}" : GameSettings.version.ToString());
        SetFPS();
        uiDebugScreen.SetPlayerPos(playerPos);
        uiDebugScreen.SetInChunkPos(new(playerPos.x - chunkPos.x * ChunkSettings.width, playerPos.y, playerPos.z - chunkPos.y * ChunkSettings.width));
        uiDebugScreen.SetChunkPos(chunkPos);
        uiDebugScreen.SetDirection(playerCharacter.GetDirection());

        SetMemory();
        uiDebugScreen.SetCPU(SystemInfo.processorType);
        uiDebugScreen.SetGPU(SystemInfo.graphicsDeviceName);
        SetTargetBlock();

        first = false;
    }

    void SetTargetBlock()
    {
        if (playerInteractions.interactionTargetChunkRenderer != null)
            uiDebugScreen.SetTargetBlock(playerInteractions.interactionTargetChunkRenderer.chunkData.GetBlock(playerInteractions.interactionTarget), playerInteractions.interactionTarget);
        else uiDebugScreen.SetTargetBlock();
    }

    void SetMemory()
    {
        uiDebugScreen.SetMemory(Profiler.GetTotalAllocatedMemoryLong() / (1024f * 1024f), Profiler.GetTotalReservedMemoryLong() / (1024f * 1024f));
    }

    void SetFPS()
    {
        _accumulatedTime += Time.unscaledDeltaTime;
        _frameCount++;

        if (_accumulatedTime >= .5f)
        {
            uiDebugScreen.SetFPS(_frameCount / _accumulatedTime);

            _accumulatedTime = 0f;
            _frameCount = 0;
        }
    }
}