public static class OptionsSettings
{
    // Render
    internal static int displayRenderDistance { get; private set; } = 8;
    internal static int realRenderDistance { get; private set; } = 7;

    // Volume
    internal static float masterVolume { get; private set; } = 1f;
    internal static float musicVolume { get; private set; } = 1f;
    internal static float playerVolume { get; private set; } = 1f;

    // Sensitivity
    internal static float sensitivity { get; private set; } = 20f;

    internal static void SetRenderDistance(int renderDistance)
    {
        displayRenderDistance = renderDistance;
        realRenderDistance = renderDistance - 1;
    }

    internal static void SaveOptions(int displayRenderDistance, float masterVolume, float musicVolume, float playerVolume, float sensitivity)
    {
        SetRenderDistance(displayRenderDistance);

        OptionsSettings.masterVolume = masterVolume;
        OptionsSettings.musicVolume = musicVolume;
        OptionsSettings.playerVolume = playerVolume;

        OptionsSettings.sensitivity = sensitivity;
    }
}