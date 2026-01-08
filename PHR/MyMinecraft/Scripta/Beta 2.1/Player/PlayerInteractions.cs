using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractions : MonoBehaviour
{
    [SerializeField] InputActionReference attackAction;
    [SerializeField] InputActionReference interactAction;

    int interactionRange = 5;

    Camera mainCamera;

    void Awake()
    {
        mainCamera = Camera.main;
    }

    void OnEnable()
    {
        attackAction.action.performed += OnAttack;
        interactAction.action.performed += OnInteract;
    }

    void OnDisable()
    {
        attackAction.action.performed -= OnAttack;
        interactAction.action.performed -= OnInteract;
    }

    void OnAttack(InputAction.CallbackContext context)
    {
        if (Physics.Raycast(new Ray(mainCamera.transform.position, mainCamera.transform.forward), out RaycastHit hit, interactionRange))
        {
            if (hit.collider.TryGetComponent<ChunkRenderer>(out ChunkRenderer chunkRenderer))
            {
                Vector3Int hitBlockPos = Chunk.GetRaycastBlockPosition(hit, chunkRenderer.chunkData.realPosition);

                chunkRenderer.chunkData.SetBlock(hitBlockPos, BlockType.Air);

                if (Chunk.IsBlockOnEdge(hitBlockPos))
                {
                    foreach (Vector2Int neighbourWorldPos in Chunk.GetEdgeNeighboursWorldPos(chunkRenderer.chunkData.worldPosition, hitBlockPos))
                    {
                        ChunkRenderer neighbourChunkRenderer = WorldData.chunkRenderer[neighbourWorldPos];
                        neighbourChunkRenderer.RenderChunk(Chunk.GetChunkMeshData(neighbourChunkRenderer.chunkData));
                    }
                }
                chunkRenderer.RenderChunk(Chunk.GetChunkMeshData(chunkRenderer.chunkData));
            }
        }
    }

    void OnInteract(InputAction.CallbackContext context)
    {
    }
}