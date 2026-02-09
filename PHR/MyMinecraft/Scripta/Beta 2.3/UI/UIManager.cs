using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] internal GameObject playerInventoryPrefab, hotbarPrefab, slotPrefab;
    [SerializeField] internal Transform inventory;
    Color origSlotColor;

    // In editor
    public void ToggleHotbar(bool isActive)
    {
        UISettings.uiHotbar.UpdateSlots();
        UISettings.uiHotbar.ToggleHotbar(isActive);
    }
    public void ToggleInventory()
    {
        UISettings.uiPlayerInventory.UpdateSlots();
    }
    // ---

    internal (Transform, TextMeshProUGUI) CreateSlot(Transform parent)
    {
        Transform slot = Instantiate(UISettings.uiManager.slotPrefab, parent).transform;
        return (slot, slot.GetChild(0).GetComponent<TextMeshProUGUI>());
    }

    internal void UpdateSlot(TextMeshProUGUI slotText, BlockType blockType, int amount)
    {
        slotText.text = $"{blockType} {amount}";
    }

    internal void StartSlotHightlight(Transform slot)
    {
        slot.GetComponent<Image>().color = new Color(1f, 1f, 1f);
    }
    internal void StopSlotHightlight(Transform slot)
    {
        slot.GetComponent<Image>().color = origSlotColor;
    }

    internal void SetOrigSlotColor(Color origSlotColor)
    {
        this.origSlotColor = origSlotColor;
    }
}