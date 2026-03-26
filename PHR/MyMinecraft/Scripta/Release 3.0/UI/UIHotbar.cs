using UnityEngine;
using UnityEngine.UI;

public class UIHotbar : MonoBehaviour
{
    internal UIInventoryData uiInventoryData { get; private set; }

    GameObject hotbar;

    void Start()
    {
        UIManager uiManager = UISettings.uiManager;

        hotbar = Instantiate(uiManager.hotbarPrefab, uiManager.uiHolder);
        ToggleHotbar(false);

        uiInventoryData = hotbar.AddComponent<UIInventoryData>().Initialize(PlayerSettings.inventoryRowLength, 1);

        RectTransform hotbarRect = hotbar.GetComponent<RectTransform>();

        HorizontalLayoutGroup layoutGroup = hotbar.GetComponent<HorizontalLayoutGroup>();
        Vector2 slotSize = uiManager.slotPrefab.GetComponent<RectTransform>().sizeDelta;

        hotbarRect.sizeDelta = new Vector2(layoutGroup.padding.left + layoutGroup.padding.right + PlayerSettings.hotbarRowLength * slotSize.x + (PlayerSettings.hotbarRowLength - 1) * layoutGroup.spacing, layoutGroup.padding.top + layoutGroup.padding.bottom + slotSize.y);
        hotbarRect.anchoredPosition = .5f * (layoutGroup.padding.top + layoutGroup.padding.bottom + slotSize.y) * Vector3.up;

        LoopTroughSlots(index => 
        {
            (uiInventoryData.slots[index], uiInventoryData.slotsText[index], uiInventoryData.slotsImage[index], uiInventoryData.slotsBackgroundImage[index]) = uiManager.CreateSlot(hotbar.transform, index);
            Destroy(uiInventoryData.slots[index].GetComponent<UIInventorySlot>());
        });

        uiManager.SetOrigSlotColor(uiManager.slotPrefab.GetComponent<Image>().color);
        uiManager.StartSlotHightlight(uiInventoryData.slotsBackgroundImage[PlayerSettings.selectedSlot]);
    }

    internal void LoopTroughSlots(System.Action<int> action)
    {
        for (int i = 0; i < uiInventoryData.slots.Length; i++)
        {
            action(i);
        }
    }

    internal void UpdateSlots()
    {
        LoopTroughSlots(index => UISettings.uiManager.UpdateSlot(uiInventoryData.slotsImage[index], uiInventoryData.slotsText[index], PlayerSettings.playerInventory.GetBlockType(index, false), PlayerSettings.playerInventory.GetBlockQuantity(index, false)));
    }

    internal void ToggleHotbar(bool isActive)
    {
        hotbar.SetActive(isActive);
    }
}