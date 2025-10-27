using System.Collections.Generic;
using UnityEngine;

public class BlockDataManager : MonoBehaviour
{
    public const float textureOffset = .001f, tileSize = .1f;

    public static Dictionary<BlockType, TextureData> blockTexture = new();
    public BlockDataSO textureData;

    private void Awake()
    {
        foreach (var item in textureData.textureDataList)
        {
            if (blockTexture.ContainsKey(item.blockType) == false)
            {
                blockTexture.Add(item.blockType, item);
            }
        }
    }
}