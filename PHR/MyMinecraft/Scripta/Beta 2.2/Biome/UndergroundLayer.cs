public class UndergroundLayer : BiomeLayerHandler
{
    public BlockType undergroundBlockType;

    protected override bool TryHandling(ChunkData chunkData, int x, int y, int z, int surfaceHeight)
    {
        if (y < surfaceHeight)
        {
            chunkData.SetBlock(x, y, z, undergroundBlockType);
            return true;
        }
        return false;
    }
}