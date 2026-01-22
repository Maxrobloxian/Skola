using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
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
}