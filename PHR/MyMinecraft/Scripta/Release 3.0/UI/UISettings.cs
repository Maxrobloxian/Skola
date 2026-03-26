public class UISettings
{
    public static UIHotbar uiHotbar;
    public static UIPlayerInventory uiPlayerInventory;
    public static UIManager uiManager;
    public static ParentCounter parentCounter;

    internal static void AddData(UIHotbar uiHotbar, UIManager uiManager, UIPlayerInventory uiPlayerInventory, ParentCounter parentCounter)
    {
        UISettings.uiHotbar = uiHotbar;
        UISettings.uiManager = uiManager;
        UISettings.uiPlayerInventory = uiPlayerInventory;
        UISettings.parentCounter = parentCounter;
    }
}