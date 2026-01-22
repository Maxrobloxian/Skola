using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerChunksManager : MonoBehaviour
{
    Transform gameManager;
    World world;

    [Header("Input Actions")]
    [SerializeField] InputActionReference chunkBorders;

    Vector3 oldPosition;
    Vector2Int oldChunk;

    DebugChunkBorders debugChunkBorders;
    bool showBorders;

    void Awake()
    {
        oldPosition = transform.position;
        oldChunk = Chunk.GetChunkPosition(GetPosXZ());
        gameManager = WorldSettings.world.GetGameManager().transform;
        debugChunkBorders = gameManager.GetComponent<DebugChunkBorders>();

        world = WorldSettings.world;
    }

    void OnEnable()
    {
        chunkBorders.action.performed += ChunkBorders;
    }

    void OnDisable()
    {
        chunkBorders.action.performed -= ChunkBorders;
    }

    void FixedUpdate()
    {
        if (oldPosition != transform.position)
        {
            if (oldChunk != Chunk.GetChunkPosition(GetPosXZ()))
            {
                oldChunk = Chunk.GetChunkPosition(GetPosXZ());

                world.UpdateWorld(GetWorldPosXZ());
            }
            oldPosition = transform.position;
        }
    }

    Vector2 GetPosXZ()
    {
        return new Vector2(transform.position.x, transform.position.z);
    }

    internal Vector2Int GetWorldPosXZ()
    {
        return new(Mathf.FloorToInt(transform.position.x / ChunkSettings.width), Mathf.FloorToInt(transform.position.z / ChunkSettings.width));
    }

    void ChunkBorders(InputAction.CallbackContext context)
    {
        showBorders = !showBorders;
        if (showBorders) debugChunkBorders.enabled = true;
        else debugChunkBorders.enabled = false;
    }
}