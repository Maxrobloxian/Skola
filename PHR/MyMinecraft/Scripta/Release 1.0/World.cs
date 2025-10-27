using UnityEngine;

public class World : MonoBehaviour
{
    [SerializeField] GameObject chunk;

    // Get player settings
    [SerializeField] int rednerDistance = 2;

    private void /*IEnumerator*/ Start()
    {
        print("Change Mesh to 16 bit!!!");
        print("Optimize neighbour cheking");
        print("Set Water color to 0 192 255 141");
        int chunkSize = ChunkStats.width;
        for (int x = -rednerDistance; x <= rednerDistance; x++)
        {
            for (int z = -rednerDistance; z <= rednerDistance; z++)
            {
                GenChunk.GenerateChunk(chunk, new(x * chunkSize, z * chunkSize));
                //yield return new WaitForSeconds(.5f);
            }
        }
    }
}