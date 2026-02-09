public class SurfaceLayer : BiomeLayerHandler
{
    public BlockType surfaceBlockType;

    protected override bool TryHandling(ChunkData chunkData, int x, int y, int z, int surfaceHeight)
    {
        if (y == surfaceHeight)
        {
            chunkData.SetBlock(x, y, z, surfaceBlockType);
            return true;
        }
        return false;
    }
}