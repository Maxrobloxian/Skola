using UnityEngine;

public class PlayerChunksManager : MonoBehaviour
{
    Vector3 oldPosition;
    Vector2Int oldChunk;

    void Awake()
    {
        oldPosition = transform.position;
        oldChunk = Chunk.GetChunkPosition(GetPosXZ());
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
}