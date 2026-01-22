using System.Threading.Tasks;
using UnityEngine;

public class GlobalSettingsInitializer : MonoBehaviour
{
    [Header("World")]
    [SerializeField] NoiseSettings defaultNoise;
    [SerializeField] Transform biomeHandler;
    [SerializeField] DomainWarping domainWarping;
    [SerializeField] World world;

    [Header("UI")]
    [SerializeField] UIHotbar uiHotbar;
    [SerializeField] UIManager uiManager;

    // Player
    PlayerInventory playerInventory;


    TaskCompletionSource<bool> _playerReadyTrigger = new();
    private async void Awake()
    {
        WorldSettings.AddData(defaultNoise, biomeHandler, domainWarping, world);
        UISettings.AddData(uiHotbar, uiManager);

        await _playerReadyTrigger.Task;

        PlayerSettings.AddData(playerInventory);

        Destroy(this);
    }

    public void PlayerSettingsAddData(PlayerInventory playerInventory)
    {
        this.playerInventory = playerInventory;

        _playerReadyTrigger.TrySetResult(true);
    }
}