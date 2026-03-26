using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;

public static class WorldFileHandler
{
    static readonly Regex invalidCharRegex = new(
        $"[{Regex.Escape(new string(Path.GetInvalidFileNameChars()))}!]",
        RegexOptions.Compiled
    );

    static readonly Regex extraSpaceRegex = new (
        @"\s+",
        RegexOptions.Compiled
    );

    static readonly Regex reservedNameRegex = new(
        @"^(CON|PRN|AUX|NUL|COM[1-9]|LPT[1-9])$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase
    );

    internal static bool CreateNew(string worldRealName, string worldDisplayName, Vector2Int worldSeed)
    {
        if (string.IsNullOrWhiteSpace(worldDisplayName) || string.IsNullOrWhiteSpace(worldRealName))
        {
            Debug.LogWarning($"World display or real name is null ot empty.");
            return false;
        }

        worldRealName = DisplayToRealName(worldRealName);

        if (string.IsNullOrWhiteSpace(worldRealName) || reservedNameRegex.IsMatch(worldRealName))
        {
            Debug.LogWarning($"World name '{worldRealName}' is invalid or reserved by the OS. Cannot create.");
            return false;
        }

        string folderPath = $"{Application.streamingAssetsPath}/Worlds/{worldRealName}/";

        try
        {
            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine("The caller does not have the required permission to create `{0}`", folderPath);
            return false;
        }

        using BinaryWriter writer = new(new FileStream($"{folderPath}settings.dat", FileMode.Create));

        writer.Write(worldDisplayName);
        writer.Write(worldRealName);

        writer.Write(worldSeed.x);
        writer.Write(worldSeed.y);

        return true;
    }

    internal static bool Load(string worldRealName)
    {
        string filePath = $"{Application.streamingAssetsPath}/Worlds/{worldRealName}/settings.dat";

        if (!File.Exists(filePath)) return false;

        using BinaryReader reader = new(new FileStream(filePath, FileMode.Open));

        GameSettings.worldDisplayName = reader.ReadString();
        GameSettings.worldRealName = reader.ReadString();

        WorldSettings.SetWorldSeed(new Vector2Int(reader.ReadInt32(), reader.ReadInt32()));

        return true;
    }

    internal static bool DeleteWorld(string worldRealName)
    {
        string folderPath = $"{Application.streamingAssetsPath}/Worlds/{worldRealName}/";

        if (!Directory.Exists(folderPath))
        {
            Debug.LogWarning($"World name '{worldRealName}' doesn't exist.");
            return true;
        }

        for (int i = 0; i < 3; i++)
        {
            try
            {
                Directory.Delete(folderPath, true);
                //Directory.Delete(folderPath);
            }
            catch (IOException e)
            {
                Console.WriteLine("The process failed: {0}", e.Message);
                System.Threading.Thread.Sleep(50);
                return false;
            }
        }

        if (Directory.Exists(folderPath))
        {
            Debug.LogWarning($"World name '{worldRealName}' couldn't be deleted.");
            return false;
        }

        return true;
    }

    internal static string GetWorldDisplayName(string worldNamePathPath)
    {
        string filePath = $"{worldNamePathPath}/settings.dat";

        if (!File.Exists(filePath))
        {
            Debug.LogWarning($"File: {worldNamePathPath}/settings.dat doesn't exist");
            return null;
        }
        using BinaryReader reader = new(new FileStream(filePath, FileMode.Open));

        string temp = reader.ReadString();
        return temp;
    }

    internal async static Task<float?> GetWorldSize(string worldNamePathPath)
    {
        if (!File.Exists($"{worldNamePathPath}/settings.dat")) return null;

        return await Task.Run(() =>
        {
            long bytes = 0;
            var files = Directory.EnumerateFiles(worldNamePathPath, "*", SearchOption.AllDirectories);
            foreach (string file in files) bytes += new FileInfo(file).Length;

            return bytes / 1048576;
        });
    }

    internal static string DisplayToRealName(string worldDisplayName)
    {
        return extraSpaceRegex.Replace(invalidCharRegex.Replace(worldDisplayName, ""), " ").Trim();
    }
}