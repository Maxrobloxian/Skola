using UnityEngine;

public class StoneLayer : BiomeLayerHandler
{
    [Range(0, 1)][SerializeField] float stoneThreshold = 0.5f;

    [SerializeField] NoiseSettings stoneNoiseSettings;

    protected override bool TryHandling(ChunkData chunkData, int x, int y, int z, int groundHeight)
    {
        if (WorldSettings.domainWarping.GenerateDomainNoise(chunkData.realPosition.x + x, chunkData.realPosition.y + z, stoneNoiseSettings) > stoneThreshold)
        {
            for (int i = 0; i <= groundHeight; i++)
            {
                chunkData.SetBlock(x, i, z, BlockType.Stone);
            }
            return true;
        }
        return false;
    }
}