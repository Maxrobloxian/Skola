using System.IO;
using UnityEngine;

public static class PlayerFileHandler
{
    internal static void Save()
    {
        string folderPath = $"{Application.streamingAssetsPath}/Worlds/{GameSettings.worldRealName}/";

        if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

        using BinaryWriter writer = new(new FileStream($"{folderPath}player.dat", FileMode.Create));

        writer.Write(PlayerSettings.playerTransform.position.x);
        writer.Write(PlayerSettings.playerTransform.position.y);
        writer.Write(PlayerSettings.playerTransform.position.z);

        writer.Write(PlayerSettings.playerCamera.transform.localEulerAngles.y);

        writer.Write(Camera.main.transform.localEulerAngles.x);
    }

    internal static (float, float)? LoadRotations()
    {
        string filePath = $"{Application.streamingAssetsPath}/Worlds/{GameSettings.worldRealName}/player.dat";

        if (!File.Exists(filePath)) return null;

        using BinaryReader reader = new(new FileStream(filePath, FileMode.Open));

        reader.BaseStream.Position += 12;

        return (reader.ReadSingle(), reader.ReadSingle());
    }

    internal static Vector3? LoadPosition()
    {
        string filePath = $"{Application.streamingAssetsPath}/Worlds/{GameSettings.worldRealName}/player.dat";

        if (!File.Exists(filePath)) return null;

        using BinaryReader reader = new(new FileStream(filePath, FileMode.Open));

        return new(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
    }
}