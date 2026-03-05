using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class StructureGenerator : MonoBehaviour
{
    bool duplicate;

    public void GenStructure()
    {
        duplicate = false;
        StructureData structureData = new();
        HashSet<Vector3Int> hashBlockPositions = new();

        StructureBlockData tempBlockData;
        for (int i = 0; i < transform.childCount; i++)
        {
            tempBlockData = transform.GetChild(i).GetComponent<StructureBlockData>();
            if (hashBlockPositions.Add(tempBlockData.position)) 
            {
                structureData.positions.Add(tempBlockData.position);
                structureData.blockTypes.Add(tempBlockData.blockType);
            }
            else
            {
                duplicate = true;
                Debug.LogWarning($"Duplicate: {tempBlockData.name} -> {tempBlockData.position}");
            }
        }

        if (duplicate) return;

        string folderPath = Application.streamingAssetsPath + "/Structures/";

        if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

        File.WriteAllText(folderPath + transform.name + ".json", JsonUtility.ToJson(structureData, true));

#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif

        print($"Saved structure to: {folderPath}{transform.name}.json");
    }

    [System.Serializable]
    public class StructureData
    {
        public List<Vector3Int> positions = new();
        public List<BlockType> blockTypes = new();
    }
}