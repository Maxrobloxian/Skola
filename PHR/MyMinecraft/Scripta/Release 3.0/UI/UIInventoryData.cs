using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryData : MonoBehaviour
{
    internal Transform[] slots { get; private set; }
    internal TextMeshProUGUI[] slotsText { get; private set; }
    internal Image[] slotsImage { get; private set; }
    internal Image[] slotsBackgroundImage { get; private set; }

    internal UIInventoryData Initialize(int rowLength, int rows)
    {
        slots = new Transform[rowLength * rows];
        slotsText = new TextMeshProUGUI[rowLength * rows];
        slotsImage = new Image[rowLength * rows];
        slotsBackgroundImage = new Image[rowLength * rows];

        return this;
    }
}