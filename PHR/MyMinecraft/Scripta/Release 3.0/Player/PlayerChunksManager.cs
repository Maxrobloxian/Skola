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
        oldChunk = Chunk.GetChunkWorldPosition(playerCharacter.GetPosXZ());

        world = WorldSettings.world;
    }

    void FixedUpdate()
    {
        if (oldPosition != transform.position)
        {
            if (oldChunk != Chunk.GetChunkWorldPosition(playerCharacter.GetPosXZ()))
            {
                oldChunk = Chunk.GetChunkWorldPosition(playerCharacter.GetPosXZ());

                world.UpdateWorld();
            }
            oldPosition = transform.position;
        }
    }
}