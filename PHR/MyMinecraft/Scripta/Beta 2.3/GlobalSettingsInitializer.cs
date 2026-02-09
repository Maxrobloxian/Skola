using System.Threading.Tasks;
using UnityEngine;

public class GlobalSettingsInitializer : MonoBehaviour
{
    [Header("World")]
    [SerializeField] NoiseSettings defaultNoise;
    [SerializeField] Transform biomeHandler;
    [SerializeField] DomainWarping domainWarping;
    [SerializeField] World world;

    [Header("Game")]
    [SerializeField] GameManager gameManager;

    [Header("UI")]
    [SerializeField] UIHotbar uiHotbar;
    [SerializeField] UIManager uiManager;
    [SerializeField] UIPlayerInventory uiInventory;


    readonly TaskCompletionSource<bool> playerReadyTrigger = new();
    private async void Awake()
    {
        GameSettings.AddData(gameManager);
        WorldSettings.AddData(defaultNoise, biomeHandler, domainWarping, world);
        UISettings.AddData(uiHotbar, uiManager, uiInventory);

        await playerReadyTrigger.Task;

        Destroy(this);
    }

    public void PlayerSettingsAddData(Transform playerTransfrom)
    {
        PlayerSettings.AddData(
            playerTransfrom.GetComponent<PlayerInventory>(),
            playerTransfrom.GetComponent<PlayerInputs>(),
            playerTransfrom.GetComponent<PlayerChunksManager>(),
            playerTransfrom.GetComponent<PlayerCharacter>(),
            playerTransfrom.GetComponent<PlayerInteractions>()
        );

        playerReadyTrigger.TrySetResult(true);
    }
}