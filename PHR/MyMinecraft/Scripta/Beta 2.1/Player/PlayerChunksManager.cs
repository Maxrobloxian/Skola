using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerChunksManager : MonoBehaviour
{
    [SerializeField] InputActionReference chunkBorders;

    Vector3 oldPosition;
    Vector2Int oldChunk;

    bool showBorders;

    void Awake()
    {
        oldPosition = transform.position;
        oldChunk = Chunk.GetChunkPosition(GetPosXZ());
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

                WorldSettings.world.UpdateWorld(GetWorldPosXZ());
            }
            oldPosition = transform.position;
        }
    }

    Vector2 GetPosXZ()
    {
        return new Vector2(transform.position.x, transform.position.z);
    }

    Vector2Int GetWorldPosXZ()
    {
        return new(Mathf.FloorToInt(transform.position.x / ChunkStats.width), Mathf.FloorToInt(transform.position.z / ChunkStats.width));
    }

    void ChunkBorders(InputAction.CallbackContext context)
    {
        showBorders = !showBorders;
        if (showBorders) foreach (ChunkRenderer chunkRenderer in WorldData.chunkRenderer.Values)
        {
            if (GetWorldPosXZ() == Vector2Int.zero) Gizmos.color = Color.red;
            else Gizmos.color = Color.yellow;
            Gizmos.DrawCube(chunkRenderer.transform.position + new Vector3(ChunkStats.width * .5f, ChunkStats.height * .5f, ChunkStats.width * .5f), new Vector3(ChunkStats.width, ChunkStats.height, ChunkStats.width));
        }
    }
}