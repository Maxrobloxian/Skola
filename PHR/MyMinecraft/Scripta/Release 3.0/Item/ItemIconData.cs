using System.Collections.Generic;
using UnityEngine;

public static class ItemIconData
{
    public static readonly Dictionary<BlockType, Sprite> icons = new();

    internal static void DeleteIcons()
    {
        icons.Clear();
    }
}