using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerInventory : MonoBehaviour
{
    internal UIInventoryData uiInventoryData { get; private set; }
    internal UIInventoryData uiDraggedSlotData { get; private set; }

    GameObject playerInventory;
    internal int oldSlotIndex { get; private set; }

    bool isDraging;


    private void Start()
    {
        uiDraggedSlotData = gameObject.AddComponent<UIInventoryData>().Initialize(1, 1);

        UIManager uiManager = UISettings.uiManager;

        playerInventory = Instantiate(uiManager.playerInventoryPrefab, uiManager.menu);
        ToggleInventory(false);

        uiInventoryData = playerInventory.AddComponent<UIInventoryData>().Initialize(PlayerSettings.inventoryRowLength, PlayerSettings.inventoryRows);

        RectTransform inventoryRect = playerInventory.GetComponent<RectTransform>();

        GridLayoutGroup layoutGroup = playerInventory.transform.GetChild(0).GetComponent<GridLayoutGroup>();
        layoutGroup.cellSize = uiManager.slotPrefab.GetComponent<RectTransform>().sizeDelta;

        inventoryRect.sizeDelta = new Vector2(layoutGroup.padding.left + layoutGroup.padding.right + PlayerSettings.inventoryRowLength * layoutGroup.cellSize.x + (PlayerSettings.inventoryRowLength - 1) * layoutGroup.spacing.x,
                                              layoutGroup.padding.top + layoutGroup.padding.bottom + PlayerSettings.inventoryRows * layoutGroup.cellSize.y + (PlayerSettings.inventoryRows - 1) * layoutGroup.spacing.y + 5);
        inventoryRect.anchoredPosition = .5f * (layoutGroup.padding.top + layoutGroup.padding.bottom + layoutGroup.cellSize.y) * Vector3.up;

        Transform[] slotParents = new Transform[2] { playerInventory.transform.GetChild(0), playerInventory.transform.GetChild(1) };

        LoopTroughSlots(index => (uiInventoryData.slots[index], uiInventoryData.slotsText[index], uiInventoryData.slotsImage[index], uiInventoryData.slotsBackgroundImage[index]) = uiManager.CreateSlot(slotParents[index < 9 ? 1 : 0], index));

        (uiDraggedSlotData.slots[0], uiDraggedSlotData.slotsText[0], uiDraggedSlotData.slotsImage[0]) = CreateDraggingSlot(uiManager.draggingSlotPrefab, transform);
        uiDraggedSlotData.slots[0].gameObject.SetActive(false);
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

    internal void ToggleInventory()
    {
        ToggleInventory(!playerInventory.activeSelf);
    }
    internal void ToggleInventory(bool isActive)
    {
        if (!isActive && isDraging) CancelDragging();

        playerInventory.SetActive(isActive);
    }

    void StartDragging(int slotIndex, Sprite slotSprite, string slotText)
    {
        isDraging = true;

        oldSlotIndex = slotIndex;

        UISettings.uiManager.UpdateDraggingSlot(uiDraggedSlotData, slotSprite, slotText);
        uiDraggedSlotData.slots[0].gameObject.SetActive(true);
    }
    void StopDragging()
    {
        uiDraggedSlotData.slots[0].gameObject.SetActive(false);
        isDraging = false;
    }
    internal void SwapDragging(int slotIndex, Sprite slotSprite, string slotText)
    {
        PlayerSettings.playerInventory.SwapDraggingAndInventory(PlayerSettings.playerInventory.inventorySlots, slotIndex, uiInventoryData);

        UISettings.uiManager.UpdateDraggingSlot(uiDraggedSlotData, slotSprite, slotText);
    }
    internal void CancelDragging()
    {
        ChangeDraggingAmmount(oldSlotIndex, -PlayerSettings.playerInventory.draggingSlot.slots[0].amount);
    }
    internal bool GetIsDragging()
    {
        return isDraging;
    }

    (Transform, TextMeshProUGUI, Image) CreateDraggingSlot(GameObject draggingSlotPrefab, Transform parent)
    {
        Transform slot = Instantiate(draggingSlotPrefab, parent).transform;
        return (slot, slot.GetChild(0).GetComponent<TextMeshProUGUI>(), slot.GetComponent<Image>());
    }

    internal bool ChangeDraggingAmmount(int slotIndex, int amount)
    {
        if (amount < 0)
        {
            if (PlayerSettings.playerInventory.draggingSlot.slots[0].blockType == PlayerSettings.playerInventory.inventorySlots.slots[slotIndex].blockType || PlayerSettings.playerInventory.inventorySlots.slots[slotIndex].blockType == BlockType.Nothing)
                PlayerSettings.playerInventory.MoveFromDraggingSlot(PlayerSettings.playerInventory.inventorySlots, slotIndex, -amount, uiInventoryData, uiDraggedSlotData);
            else return false;
        }
        else PlayerSettings.playerInventory.MoveToDraggingSlot(PlayerSettings.playerInventory.inventorySlots, slotIndex, amount, uiInventoryData, uiDraggedSlotData);

        if (PlayerSettings.playerInventory.draggingSlot.slots[0].amount <= 0) StopDragging();
        return true;
    }
    internal void ChangeDraggingAmmount(int slotIndex, Sprite slotSprite, int amount)
    {
        if (ChangeDraggingAmmount(slotIndex, amount))
            StartDragging(slotIndex, slotSprite, amount.ToString());
    }
}