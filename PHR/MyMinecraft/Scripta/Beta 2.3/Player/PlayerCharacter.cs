using System;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    Transform eyes;

    private void Awake()
    {
        eyes = GetComponent<PlayerMovement>().GetEyes();
    }

    internal void ChangeCharacterSize(float playerHeight)
    {
        float moveAmount = playerHeight - transform.localScale.y;
        if (moveAmount < 0)
        {
            transform.localScale = new(transform.localScale.x, playerHeight, transform.localScale.z);
            rb.position += .5f * moveAmount * Vector3.up;
        }
        else
        {
            rb.position += .5f * moveAmount * Vector3.up;
            transform.localScale = new(transform.localScale.x, playerHeight, transform.localScale.z);
        }
    }

    internal Vector2 GetPosXZ()
    {
        return new Vector2(transform.position.x, transform.position.z);
    }
    internal Vector2Int GetWorldPosXZ()
    {
        return new(Mathf.FloorToInt(transform.position.x / ChunkSettings.width), Mathf.FloorToInt(transform.position.z / ChunkSettings.width));
    }

    internal Vector3Int GetPosToInt()
    {
        return new(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y), Mathf.FloorToInt(transform.position.z));
    }

    internal string GetDirection()
    {
        return eyes.transform.GetCardinalDirectionXZ() switch
        {
            Direction.right => "East",
            Direction.left => "West",
            Direction.forward => "North",
            Direction.backward => "South",
            _ => throw new Exception("Invalid input direction")
        };
    }
}