using UnityEngine;

public class ShootingPadManager : MonoBehaviour
{
    public bool inPosition;

    // Pokud je clovek na sve posici pise ze tam je
    private void OnTriggerEnter(Collider other)
    {
        inPosition = true;
    }

    // Pokud clovek neni na sve posici pise ze neni
    private void OnTriggerExit(Collider other)
    {
        inPosition = false;
    }
}