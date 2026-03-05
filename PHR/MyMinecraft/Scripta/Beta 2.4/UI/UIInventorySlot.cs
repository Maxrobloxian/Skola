using UnityEngine;
using UnityEngine.EventSystems;

public class UIInventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    int slotIndex;
    UIInventoryData inventoryData;

    private void Awake()
    {
        inventoryData = transform.parent.GetComponentInParent<UIInventoryData>();
    }
    private void OnDisable()
    {
        UISettings.uiManager.StopSlotHightlight(inventoryData.slotsBackgroundImage[slotIndex]);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UISettings.uiManager.StartSlotHightlight(inventoryData.slotsBackgroundImage[slotIndex]);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        UISettings.uiManager.StopSlotHightlight(inventoryData.slotsBackgroundImage[slotIndex]);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left) UISettings.uiManager.LeftClickInventorySlot(inventoryData.slotsImage[slotIndex].sprite, inventoryData.slotsText[slotIndex].text, slotIndex);
        else UISettings.uiManager.RightClickInventorySlot(inventoryData.slotsImage[slotIndex].sprite, slotIndex);
    }

    internal void SetSlotIndex(int slotIndex)
    {
        this.slotIndex = slotIndex;
    }
    internal int GetSlotIndex()
    {
        return slotIndex;
    }
}