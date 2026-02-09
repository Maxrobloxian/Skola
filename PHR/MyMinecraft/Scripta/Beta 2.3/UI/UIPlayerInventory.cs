using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerInventory : MonoBehaviour
{
    readonly Transform[] slots = new Transform[PlayerSettings.inventoryRowLength * PlayerSettings.inventoryRows];
    readonly TextMeshProUGUI[] slotsText = new TextMeshProUGUI[PlayerSettings.inventoryRowLength * PlayerSettings.inventoryRows];

    [SerializeField] GameObject playerInventory;

    private void Start()
    {
        UIManager uiManager = UISettings.uiManager;

        playerInventory = Instantiate(uiManager.playerInventoryPrefab, uiManager.inventory);
        ToggleInventory(false);

        RectTransform inventoryRect = playerInventory.GetComponent<RectTransform>();

        GridLayoutGroup layoutGroup = playerInventory.GetComponent<GridLayoutGroup>();
        layoutGroup.cellSize = uiManager.slotPrefab.GetComponent<RectTransform>().sizeDelta;

        inventoryRect.sizeDelta = new Vector2(layoutGroup.padding.left + layoutGroup.padding.right + PlayerSettings.inventoryRowLength * layoutGroup.cellSize.x + (PlayerSettings.inventoryRowLength - 1) * layoutGroup.spacing.x,
                                              layoutGroup.padding.top + layoutGroup.padding.bottom + PlayerSettings.inventoryRows * layoutGroup.cellSize.y + (PlayerSettings.inventoryRows - 1) * layoutGroup.spacing.y);
        inventoryRect.anchoredPosition = .5f * (layoutGroup.padding.top + layoutGroup.padding.bottom + layoutGroup.cellSize.y) * Vector3.up;

        LoopTroughSlots(index => (slots[index], slotsText[index]) = uiManager.CreateSlot(playerInventory.transform));
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

    internal void ToggleInventory()
    {
        playerInventory.SetActive(!playerInventory.activeSelf);
    }
    internal void ToggleInventory(bool isActive)
    {
        playerInventory.SetActive(isActive);
    }

    internal TextMeshProUGUI GetSlotText(int slotIndex)
    {
        return slotsText[slotIndex];
    }
}