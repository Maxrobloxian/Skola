using UnityEngine;
using TMPro;

public class WorldUIManager : MonoBehaviour
{
    // Ruzne floaty, texty a objecty
    public float originalHPValue;
    public float actualHPValue;
    [SerializeField] private TextMeshPro originalHP;
    [SerializeField] private TextMeshPro actualHP;
    [SerializeField] private GameObject stopButton;

    // Ujistuje ze text ukazuje originalni pocet zivotu a actualni pocet zivotu, a pokud je hra stopnuta tak ukazuje 0
    void Update()
    {
        originalHP.text = "Original HP: " + originalHPValue;
        actualHP.text = "Actual HP: " + actualHPValue;
        if (!stopButton.activeSelf) { actualHP.text = "Actual HP: 0"; }
    }
}