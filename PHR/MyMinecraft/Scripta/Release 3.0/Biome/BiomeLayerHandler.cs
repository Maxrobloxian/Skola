using UnityEngine;

public abstract class BiomeLayerHandler : MonoBehaviour
{
    [SerializeField] BiomeLayerHandler NextLayer;

    public bool Handle(ChunkData chunkData, int x, int y, int z, int groundHeight)
    {
        if (TryHandling(chunkData, x, y, z, groundHeight)) return true;

        if (NextLayer != null) return NextLayer.Handle(chunkData, x, y, z, groundHeight);
        
        return false;
    }

    protected abstract bool TryHandling(ChunkData chunkData, int x, int y, int z, int surfaceHeight);
}