using UnityEngine;
using UnityEngine.InputSystem;

public class DebugManager : MonoBehaviour
{
    PlayerInputs playerInputs;

    DebugChunkBorders debugChunkBorders;
    DebugScreen debugScreen;

    bool isActionCombo;

    private void Awake()
    {
        debugChunkBorders = GetComponent<DebugChunkBorders>();
        debugScreen = GetComponent<DebugScreen>();

#if UNITY_EDITOR
        if (debugChunkBorders.enabled) print("Turn DebugChunkBorders off!!!");
        if (debugScreen.enabled) print("Turn DebugScreen off!!!");
#endif
    }

    void OnEnable()
    {
        if (playerInputs != null) playerInputs.ToggleDebugManager(true, this);
    }

    void OnDisable()
    {
        playerInputs.ToggleDebugManager(false, this);
    }

    internal void ToggleChunkBorders(InputAction.CallbackContext context)
    {
        isActionCombo = true;
        debugChunkBorders.enabled = !debugChunkBorders.enabled;
    }

    internal void ToggleDebugScreen(InputAction.CallbackContext context)
    {
        if (isActionCombo)
        {
            isActionCombo = false;
            return;
        }
        
        debugScreen.enabled = !debugScreen.enabled;
    }

    // In editor
    public void AddPlayerData()
    {
        playerInputs = PlayerSettings.playerInputs;

        playerInputs.ToggleDebugManager(true, this);
    }
}