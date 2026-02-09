public class UISettings
{
    public static UIHotbar uiHotbar;
    public static UIPlayerInventory uiPlayerInventory;
    public static UIManager uiManager;

    internal static void AddData(UIHotbar uiHotbar, UIManager uiManager, UIPlayerInventory uiInventory)
    {
        UISettings.uiHotbar = uiHotbar;
        UISettings.uiManager = uiManager;
        UISettings.uiPlayerInventory = uiInventory;
    }
}