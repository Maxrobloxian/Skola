using System;
using UnityEngine;

public static class DirectionExtensions
{
    public static Vector3Int GetVector(this Direction direction)
    {
        return direction switch
        {
            Direction.up => Vector3Int.up,
            Direction.down => Vector3Int.down,
            Direction.right => Vector3Int.right,
            Direction.left => Vector3Int.left,
            Direction.forward => Vector3Int.forward,
            Direction.backward => Vector3Int.back,
            _ => throw new Exception("Invalid input direction")
        };
    }

    public static readonly Direction[] directions =
{
        Direction.backward,
        Direction.down,
        Direction.forward,
        Direction.left,
        Direction.right,
        Direction.up
    };

    public static Direction GetCardinalDirection(this Transform transform)
    {
        Vector3 vector = transform.forward;

        if (vector.y > 0.9f) return Direction.up;
        if (vector.y < -0.9f) return Direction.down;

        vector.y = 0; 
        vector.Normalize();

        if (Mathf.Abs(vector.x) > Mathf.Abs(vector.z))
        {
            return vector.x > 0 ? Direction.right : Direction.left;
        }
        else
        {
            return vector.z > 0 ? Direction.forward : Direction.backward;
        }
    }

    public static Direction GetCardinalDirectionXZ(this Transform transform)
    {
        Vector3 vector = transform.forward;

        vector.y = 0;
        vector.Normalize();

        if (Mathf.Abs(vector.x) > Mathf.Abs(vector.z))
        {
            return vector.x > 0 ? Direction.right : Direction.left;
        }
        else
        {
            return vector.z > 0 ? Direction.forward : Direction.backward;
        }
    }
}