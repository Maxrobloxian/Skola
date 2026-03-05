using UnityEngine;

public class StructureBlockData : MonoBehaviour
{
    public Vector3Int position;
    public BlockType blockType;

    private void OnValidate()
    {
        position = new Vector3Int(
            Mathf.RoundToInt(transform.localPosition.x),
            Mathf.RoundToInt(transform.localPosition.y),
            Mathf.RoundToInt(transform.localPosition.z)
        );
    }
}