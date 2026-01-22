using UnityEngine;

public class UIManager : MonoBehaviour
{
    internal void ToggleHotbar(bool isActive)
    {
        UISettings.uiHotbar.ToggleHotbar(isActive);
        UISettings.uiHotbar.SetPlayerInventory();
        UISettings.uiHotbar.UpdateSlots();
    }
}