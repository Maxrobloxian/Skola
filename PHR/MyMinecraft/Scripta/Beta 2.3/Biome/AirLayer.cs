public class AirLayer : BiomeLayerHandler
{
    protected override bool TryHandling(ChunkData chunkData, int x, int y, int z, int surfaceHeight)
    {
        if (y > surfaceHeight)
        {
            chunkData.SetBlock(x, y, z, BlockType.Air);
            return true;
        }
        return false;
    }
}