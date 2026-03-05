using System.Collections;
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

        StartCoroutine(WaitForWorldGen());
    }

    internal async void UpdateWorld(Vector2Int playerWorldPosition)
    {
        await worldGenerate.GenWorld(playerWorldPosition);
    }

    internal GameManager GetGameManager()
    {
        return gameManager;
    }

    IEnumerator WaitForWorldGen()
    {
        while (transform.childCount < Mathf.Pow(GameSettings.renderDistance * 2 + 1, 2) && transform.childCount < 121) yield return new WaitForEndOfFrame();
        gameManager.SpawnPlayer();
    }
}