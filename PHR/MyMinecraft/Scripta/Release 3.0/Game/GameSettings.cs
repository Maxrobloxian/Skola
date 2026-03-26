using UnityEngine;

public static class GameSettings
{
    public static string worldDisplayName;
    public static string worldRealName;

    public const string versionType = "Release";
    public static readonly System.Version version = new(3, 0, 0);

    public static GameManager gameManager;
    public static Transform gameManagerTransform;

    internal static void AddData(GameManager gameManager)
    {
        GameSettings.gameManager = gameManager;
        gameManagerTransform = gameManager.transform;
    }
}