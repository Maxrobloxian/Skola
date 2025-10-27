using System.Collections.Generic;
using UnityEngine;

public static class DirectionVectors
{
    public static readonly Dictionary<Direction, Vector3Int> vector3 = new()
    {
        { Direction.forward, Vector3Int.forward },
        { Direction.right, Vector3Int.right },
        { Direction.backward, Vector3Int.back },
        { Direction.left, Vector3Int.left },
        { Direction.up, Vector3Int.up },
        { Direction.down, Vector3Int.down }
    };

    public static readonly Dictionary<Direction, Vector2Int> XZ = new()
    {
        { Direction.forward, Vector2Int.up },
        { Direction.backward, Vector2Int.down },
        { Direction.right, Vector2Int.right },
        { Direction.left, Vector2Int.left }
    };

    public static readonly Dictionary<Direction, Vector2Int> chunkDirectionVector2 = new()
    {
        { Direction.forward, new(0, ChunkStats.width-1) },
        { Direction.right, new(ChunkStats.width-1, 0) },
        { Direction.backward, Vector2Int.zero },
        { Direction.left, Vector2Int.zero }
    };
}