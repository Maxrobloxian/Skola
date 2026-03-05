using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Prefabs")][SerializeField] internal GameObject playerInventoryPrefab;
    [SerializeField] internal GameObject hotbarPrefab, slotPrefab, draggingSlotPrefab;

    [Header("Objects")]
    [SerializeField] GameObject pauseMenu;

    [Header("Transforms")][SerializeField] internal Transform menu;
    [SerializeField] internal Transform uiHolder;

    [Header("Misc")]
    [SerializeField] ParentCounter parentCounter;

    Color origSlotColor;

    PlayerInputs playerInputs;

    void OnEnable()
    {
        if (playerInputs != null) playerInputs.ToggleUIManager(true, this);
    }

    void OnDisable()
    {
        playerInputs.ToggleUIManager(false, this);
    }

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

    public void AddPlayerData()
    {
        playerInputs = PlayerSettings.playerInputs;

        playerInputs.ToggleUIManager(true, this);
    }
    // ---

    internal (Transform, TextMeshProUGUI, Image, Image) CreateSlot(Transform parent, int slotIndex)
    {
        Transform slot = Instantiate(slotPrefab, parent).transform;
        slot.AddComponent<UIInventorySlot>().SetSlotIndex(slotIndex);
        return (slot, slot.GetChild(1).GetComponent<TextMeshProUGUI>(), slot.GetChild(0).GetComponent<Image>(), slot.GetComponent<Image>());
    }

    internal void UpdateSlot(Image slotImage, TextMeshProUGUI slotText, BlockType blockType, int amount)
    {
        if (blockType == BlockType.Nothing)
        {
            slotImage.enabled = false;

            slotImage.sprite = null;
            slotText.text = null;
            return;
        }

        slotImage.sprite = ItemIconData.icons[blockType];
        slotText.text = amount.ToString();

        slotImage.enabled = true;
    }
    internal void UpdateSlot(Image slotImage, TextMeshProUGUI slotText, Sprite sprite, string text)
    {
        slotImage.sprite = sprite;
        slotText.text = text;

        slotImage.enabled = true;
    }
    internal void UpdateDraggingSlot(UIInventoryData draggedSlotData, Sprite sprite, string text)
    {
        UpdateSlot(draggedSlotData.slotsImage[0], draggedSlotData.slotsText[0], sprite, text);
    }

    internal void StartSlotHightlight(Image image)
    {
        image.color = new Color(1f, 1f, 1f);
    }
    internal void StopSlotHightlight(Image image)
    {
        image.color = origSlotColor;
    }

    internal void SetOrigSlotColor(Color origSlotColor)
    {
        this.origSlotColor = origSlotColor;
    }

    internal void LeftClickInventorySlot(Sprite slotSprite, string slotText, int slotIndex)
    {
        if (UISettings.uiPlayerInventory.GetIsDragging())
        {
            if (slotSprite)
            {
                if (UISettings.uiPlayerInventory.uiDraggedSlotData.slotsImage[0].sprite == slotSprite) 
                    UISettings.uiPlayerInventory.ChangeDraggingAmmount(slotIndex, -PlayerSettings.playerInventory.draggingSlot.slots[0].amount);
                else UISettings.uiPlayerInventory.SwapDragging(slotIndex, slotSprite, slotText);
            }
            else UISettings.uiPlayerInventory.ChangeDraggingAmmount(slotIndex, -PlayerSettings.playerInventory.draggingSlot.slots[0].amount);
        }
        else if (slotSprite)
        {
            UISettings.uiPlayerInventory.ChangeDraggingAmmount(slotIndex, slotSprite, PlayerSettings.playerInventory.inventorySlots.slots[slotIndex].amount);
        }
    }
    internal void RightClickInventorySlot(Sprite slotSprite, int slotIndex)
    {
        if (UISettings.uiPlayerInventory.GetIsDragging())
        {
            UISettings.uiPlayerInventory.ChangeDraggingAmmount(slotIndex, -1);
        }
        else if (slotSprite)
        {
            UISettings.uiPlayerInventory.ChangeDraggingAmmount(slotIndex, slotSprite, Mathf.CeilToInt(PlayerSettings.playerInventory.inventorySlots.slots[slotIndex].amount * .5f));
        }
    }

    internal void BackAction(InputAction.CallbackContext context)
    {
        if (parentCounter.GetActiveCount() <= 0) TooglePauseMenu(true);
        else for (int i = 0; i < menu.childCount; i++)
            {
                TooglePauseMenu(false);
                menu.GetChild(i).gameObject.SetActive(false);
            }
    }

    // + In editor
    public void TooglePauseMenu(bool pause)
    {
        GameSettings.gameManager.TogglePause(pause);

        pauseMenu.SetActive(pause);
    }
}