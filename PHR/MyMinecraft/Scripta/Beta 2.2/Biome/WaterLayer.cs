public class WaterLayer : BiomeLayerHandler
{
    protected override bool TryHandling(ChunkData chunkData, int x, int y, int z, int surfaceHeight)
    {
        if (y > surfaceHeight && y <= WorldSettings.waterHeight)
        {
            chunkData.SetBlock(x, y, z, BlockType.Water);
            if (y == surfaceHeight + 1)
            {
                chunkData.SetBlock(x, surfaceHeight, z, BlockType.Sand);
            }
            return true;
        }
        return false;
    }
}