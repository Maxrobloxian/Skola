using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHotbar : MonoBehaviour
{
    [SerializeField] GameObject hotbarPrefab, slotPrefab;

    PlayerInventory playerInventory;

    Color slotColor;

    Image slotImage;

    GameObject hotbar;
    [SerializeField] List<Transform> slots = new();
    [SerializeField] List<TextMeshProUGUI> slotsText = new();
    //readonly List<Transform> slots = new();
    //readonly List<TextMeshProUGUI> slotsText = new();

    void Awake()
    {
        hotbar = Instantiate(hotbarPrefab, transform);
        ToggleHotbar(false);

        RectTransform hotbarRect = hotbar.GetComponent<RectTransform>();

        Vector2 slotSize = slotPrefab.GetComponent<RectTransform>().sizeDelta;
        HorizontalLayoutGroup layoutGroup = hotbar.GetComponent<HorizontalLayoutGroup>();

        hotbarRect.sizeDelta = new Vector2(layoutGroup.padding.left + layoutGroup.padding.right + PlayerSettings.hotbarRowLength * slotSize.x + (PlayerSettings.hotbarRowLength - 1) * layoutGroup.spacing, layoutGroup.padding.top + layoutGroup.padding.bottom + slotSize.y);
        hotbarRect.anchoredPosition = .5f * (layoutGroup.padding.top + layoutGroup.padding.bottom + slotSize.y) * Vector3.up;

        LoopTroughSlots(CreateSlot);

        slotImage = slots[0].GetComponent<Image>();
        slotColor = slotImage.color;
        HightlightSlot(0);
    }

    internal Transform GetSlot(int slotIndex)
    {
        return slots[slotIndex];
    }

    internal void HightlightSlot(int slotIndex)
    {
        slotImage.color = slotColor;

        slotImage = slots[slotIndex].GetComponent<Image>();
        slotImage.color = new Color(1f, 1f, 1f);
    }

    void LoopTroughSlots(System.Action<int> action)
    {
        for (int i = 0; i < PlayerSettings.hotbarRowLength; i++)
        {
            action(i);
        }
    }

    void CreateSlot(int slotIndex)
    {
        slots.Add(Instantiate(slotPrefab, hotbar.transform).transform);
        slotsText.Add(slots[slotIndex].GetChild(0).GetComponent<TextMeshProUGUI>());
    }

    internal void UpdateSlots()
    {
        LoopTroughSlots(UpdateSlot);
    }

    internal void UpdateSlot(int slotIndex)
    {
        slotsText[slotIndex].text = playerInventory.GetBlockType(slotIndex).ToString() + " " + playerInventory.GetBlockQuantity(slotIndex);
    }
    internal void UpdateSlot(int slotIndex, BlockType blockType, int amount)
    {
        slotsText[slotIndex].text = blockType.ToString() + " " + amount;
    }

    public void ToggleHotbar(bool isActive)
    {
        hotbar.SetActive(isActive);
    }

    public void SetPlayerInventory()
    {
        playerInventory = PlayerSettings.playerInventory;
    }
}