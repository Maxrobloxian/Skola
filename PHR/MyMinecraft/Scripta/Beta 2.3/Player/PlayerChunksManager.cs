using UnityEngine;

public class PlayerChunksManager : MonoBehaviour
{
    PlayerCharacter playerCharacter;
    World world;

    Vector3 oldPosition;
    internal Vector2Int oldChunk;


    void Awake()
    {
        playerCharacter = GetComponent<PlayerCharacter>();

        oldPosition = transform.position;
        oldChunk = Chunk.GetChunkPosition(playerCharacter.GetPosXZ());

        world = WorldSettings.world;
    }

    void FixedUpdate()
    {
        if (oldPosition != transform.position)
        {
            if (oldChunk != Chunk.GetChunkPosition(playerCharacter.GetPosXZ()))
            {
                oldChunk = Chunk.GetChunkPosition(playerCharacter.GetPosXZ());

                world.UpdateWorld(playerCharacter.GetWorldPosXZ());
            }
            oldPosition = transform.position;
        }
    }
}