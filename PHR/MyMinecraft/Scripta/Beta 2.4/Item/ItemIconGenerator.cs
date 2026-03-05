using UnityEngine;

public class ItemIconGenerator : MonoBehaviour
{
    Camera screenshotCamera;
    [SerializeField] BlockDataSO blockDataSO;
    [SerializeField] GameObject chunkPrefab, iconCameraPrefab;

    [SerializeField] GameObject iconBlockPrefab;

    internal void GenerateAllIcons()
    {
        Transform iconBlock = Instantiate(iconBlockPrefab, Vector3.zero, Quaternion.identity, transform).transform;
        Mesh mesh = iconBlock.GetComponent<MeshFilter>().mesh;
        MeshData meshData = new(false);

        GenCube(mesh, meshData);

        screenshotCamera = Instantiate(iconCameraPrefab, new Vector3(0.7f, 2.85f, -5f), Quaternion.Euler(25f, 0f, 0f), transform).GetComponent<Camera>();

        iconBlock.gameObject.layer = 6;
        iconBlock.transform.rotation = Quaternion.Euler(0f, 45f, 0f);

        RenderTexture renderTexture = new(256, 256, 24);
        screenshotCamera.targetTexture = renderTexture;
        RenderTexture.active = renderTexture;

        screenshotCamera.clearFlags = CameraClearFlags.SolidColor;
        screenshotCamera.backgroundColor = new Color(0, 0, 0, 0);

        Texture2D texture = new(256, 256, TextureFormat.RGBA32, false);

        foreach (TextureData textureData in blockDataSO.textureDataList)
        {
            if (textureData.blockType == BlockType.Air || textureData.blockType == BlockType.Water) continue;

            UpdateCube(mesh, meshData, textureData.blockType);

            // mayme cleanup
            screenshotCamera.Render();
            texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            texture.Apply();

            Sprite icon = Sprite.Create(texture, new Rect(0, 0, 256, 256), new Vector2(0.5f, 0.5f));
            ItemIconData.icons.Add(textureData.blockType, icon);

            texture = new Texture2D(256, 256, TextureFormat.RGBA32, false);
        }

        screenshotCamera.targetTexture = null;
        RenderTexture.active = null;

        Destroy(screenshotCamera.gameObject);
        Destroy(iconBlock.gameObject);
        Destroy(this);
    }

    void GenCube(Mesh mesh, MeshData meshData)
    {
        BlockHelper.SetBlockSide(Direction.up, 0, 0, 0, meshData, BlockType.Grass_Dirt);
        BlockHelper.SetBlockSide(Direction.right, 0, 0, 0, meshData, BlockType.Grass_Dirt);
        BlockHelper.SetBlockSide(Direction.backward, 0, 0, 0, meshData, BlockType.Grass_Dirt);

        mesh.Clear();

        mesh.vertices = meshData.vertices.ToArray();
        mesh.triangles = meshData.triangles.ToArray();
        mesh.uv = meshData.uvs.ToArray();

        mesh.RecalculateNormals();
    }

    void UpdateCube(Mesh mesh, MeshData meshData, BlockType blockType)
    {
        meshData.uvs.Clear();

        meshData.uvs.AddRange(BlockHelper.FaceUVs(Direction.up, blockType));
        meshData.uvs.AddRange(BlockHelper.FaceUVs(Direction.right, blockType));
        meshData.uvs.AddRange(BlockHelper.FaceUVs(Direction.backward, blockType));

        mesh.uv = meshData.uvs.ToArray();

        mesh.RecalculateNormals();
    }
}