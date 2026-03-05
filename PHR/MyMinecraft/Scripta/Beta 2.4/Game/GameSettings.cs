using UnityEngine;

public static class GameSettings
{
    public const string versionType = "Beta";
    public static readonly System.Version version = new(2, 3, 0);

    public static GameManager gameManager;
    public static Transform gameManagerTransform;

    // Video
    public static int renderDistance = 7; // Display + 1

    // Input
    public static float sensitivity = 20;

    internal static void AddData(GameManager gameManager)
    {
        GameSettings.gameManager = gameManager;
        gameManagerTransform = gameManager.transform;
    }
}