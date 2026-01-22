public class UISettings
{
    public static UIHotbar uiHotbar;
    public static UIManager uiManager;

    internal static void AddData(UIHotbar uiHotbar, UIManager uiManager)
    {
        UISettings.uiHotbar = uiHotbar;
        UISettings.uiManager = uiManager;
    }
}