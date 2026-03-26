using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static StructureGenerator;

public static class TreeGenerator
{
    static readonly StructureData treeData = JsonUtility.FromJson<StructureData>(File.ReadAllText($"{Application.streamingAssetsPath}/Structures/Tree.json"));

    public static void GenTrees(ChunkData chunkData)
    {
        float[,] noiseVal = new float[ChunkSettings.width, ChunkSettings.width];
        for (int x = 0; x < ChunkSettings.width; x++)
        {
            for (int z = 0; z < ChunkSettings.width; z++)
            {
                noiseVal[x, z] = WorldSettings.domainWarping.GenerateDomainNoise(chunkData.realPosition.x + x, chunkData.realPosition.y + z, WorldSettings.treeNoise);
            }
        }

        List<Vector2Int> maximas = new();
        for (int x = 0; x < ChunkSettings.width; x++)
        {
            for (int z = 0; z < ChunkSettings.width; z++)
            {
                if (CheckNeighbours(noiseVal, x, z, (neighbourNoise) => neighbourNoise < noiseVal[x, z]))
                {
                    maximas.Add(new Vector2Int(x, z));
                }
            }
        }

        foreach (Vector2Int pos in maximas) SpawnTree(chunkData, pos);

        chunkData.SetTrees();
    }

    static bool CheckNeighbours(float[,] dataMatrix, int x, int y, Func<float, bool> successCondition)
    {
        foreach (var dir in directions)
        {
            Vector2Int newPos = new(x + dir.x, y + dir.y);

            if (newPos.x < 0 || newPos.x >= ChunkSettings.width || newPos.y < 0 || newPos.y >= ChunkSettings.width) continue;

            if (successCondition(dataMatrix[x + dir.x, y + dir.y]) == false) return false;
        }
        return true;
    }

    static readonly Vector2Int[] directions =
    {
        Vector2Int.up,     //N
        Vector2Int.one,    //NE
        Vector2Int.right,  //E
        new (-1, 1),       //SE
        Vector2Int.left,   //S
        -Vector2Int.one,   //SW
        Vector2Int.down,   //W
        new ( 1,-1)        //NW
    };

    static void SpawnTree(ChunkData chunkData, Vector2Int pos)
    {
        int groundPosition = Noise.RemapValue01ToInt(Noise.Redistribution(WorldSettings.domainWarping.GenerateDomainNoise(chunkData.realPosition.x + pos.x, chunkData.realPosition.y + pos.y, WorldSettings.defaultNoise)), 0, ChunkSettings.height);
        if (groundPosition <= WorldSettings.maxTreeHeight &&
            chunkData.GetBlock(pos.x, groundPosition, pos.y) == BlockType.Grass_Dirt && chunkData.GetBlock(pos.x, groundPosition + 1, pos.y) == BlockType.Air)
        {
            chunkData.SetBlock(pos.x, groundPosition, pos.y, BlockType.Dirt);

            for (int i = 0; i < treeData.positions.Count; i++)
            {
                Vector3Int leafInChunkPos = new(pos.x + treeData.positions[i].x, groundPosition + treeData.positions[i].y, pos.y + treeData.positions[i].z);
                ChunkData newChunkData;

                (newChunkData, leafInChunkPos) = Chunk.GetOutOfBoundChunkData(chunkData, leafInChunkPos);
                if (newChunkData.GetBlock(leafInChunkPos) == BlockType.Air)
                {
                    newChunkData.SetBlock(leafInChunkPos, treeData.blockTypes[i]);
                    newChunkData.SetTreesUpdate(true);
                }
            }
        }
    }
}