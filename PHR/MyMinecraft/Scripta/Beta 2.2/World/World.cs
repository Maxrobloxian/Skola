using UnityEngine;

public class World : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    WorldGenerate worldGenerate;

    private void Start()
    {
        worldGenerate = GetComponent<WorldGenerate>();
        StartWorld();
    }

    async void StartWorld()
    {
        await worldGenerate.GenWorld(Vector2Int.zero);

        gameManager.SpawnPlayer();
    }

    internal async void UpdateWorld(Vector2Int playerWorldPosition)
    {
        await worldGenerate.GenWorld(playerWorldPosition);
    }

    internal GameManager GetGameManager()
    {
        return gameManager;
    }
}