using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHotbar : MonoBehaviour
{
    readonly Transform[] slots = new Transform[PlayerSettings.inventoryRowLength];
    readonly TextMeshProUGUI[] slotsText = new TextMeshProUGUI[PlayerSettings.inventoryRowLength];

    GameObject hotbar;

    void Start()
    {
        UIManager uiManager = GetComponent<UIManager>();

        hotbar = Instantiate(uiManager.hotbarPrefab, transform);
        ToggleHotbar(false);

        RectTransform hotbarRect = hotbar.GetComponent<RectTransform>();

        HorizontalLayoutGroup layoutGroup = hotbar.GetComponent<HorizontalLayoutGroup>();
        Vector2 slotSize = uiManager.slotPrefab.GetComponent<RectTransform>().sizeDelta;

        hotbarRect.sizeDelta = new Vector2(layoutGroup.padding.left + layoutGroup.padding.right + PlayerSettings.hotbarRowLength * slotSize.x + (PlayerSettings.hotbarRowLength - 1) * layoutGroup.spacing, layoutGroup.padding.top + layoutGroup.padding.bottom + slotSize.y);
        hotbarRect.anchoredPosition = .5f * (layoutGroup.padding.top + layoutGroup.padding.bottom + slotSize.y) * Vector3.up;

        LoopTroughSlots(index => (slots[index], slotsText[index]) = uiManager.CreateSlot(hotbar.transform));

        uiManager.SetOrigSlotColor(uiManager.slotPrefab.GetComponent<Image>().color);
        uiManager.StartSlotHightlight(GetSlot(PlayerSettings.selectedSlot));
    }

    internal void LoopTroughSlots(System.Action<int> action)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            action(i);
        }
    }

    internal void UpdateSlots()
    {
        LoopTroughSlots(index => UISettings.uiManager.UpdateSlot(slotsText[index], PlayerSettings.playerInventory.GetBlockType(index, false), PlayerSettings.playerInventory.GetBlockQuantity(index, false)));
    }

    internal void ToggleHotbar(bool isActive)
    {
        hotbar.SetActive(isActive);
    }

    internal TextMeshProUGUI GetSlotText(int slotIndex)
    {
        return slotsText[slotIndex];
    }
    internal Transform GetSlot(int slotIndex)
    {
        return slots[slotIndex];
    }
}