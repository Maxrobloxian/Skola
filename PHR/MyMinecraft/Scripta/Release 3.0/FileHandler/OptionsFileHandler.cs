using System.IO;
using UnityEngine;

public static class OptionsFileHandler
{
    static readonly string folderPath = $"{Application.streamingAssetsPath}/Options/";

    internal static void Save(int displayRenderDistance, float masterVolume, float musicVolume, float playerVolume, float sensitivity)
    {
        if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

        using BinaryWriter writer = new(new FileStream($"{folderPath}options.dat", FileMode.Create));

        writer.Write(displayRenderDistance);

        writer.Write(masterVolume);
        writer.Write(musicVolume);
        writer.Write(playerVolume);

        writer.Write(sensitivity);
    }

    internal static void Load()
    {
        string filePath = $"{folderPath}options.dat";

        if (!File.Exists(filePath))
        {
            Save(OptionsSettings.displayRenderDistance, OptionsSettings.masterVolume, OptionsSettings.musicVolume, OptionsSettings.playerVolume, OptionsSettings.sensitivity);
            return;
        }

        using BinaryReader reader = new(new FileStream(filePath, FileMode.Open));

        OptionsSettings.SaveOptions(reader.ReadInt32(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
    }
}