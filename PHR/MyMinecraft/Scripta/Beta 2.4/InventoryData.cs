using UnityEngine;

public class InventoryData : MonoBehaviour
{
    internal InventorySlot[] slots { get; private set; }

    internal InventoryData Initialize(int rowLength, int rows)
    {
        slots = new InventorySlot[rowLength * rows];

        return this;
    }
}