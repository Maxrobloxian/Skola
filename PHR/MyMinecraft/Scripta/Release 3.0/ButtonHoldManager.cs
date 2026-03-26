using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHoldManager : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    MenuGameManager menuGameManager;
    Button worldButton;
    Slider slider;

    string realWorldName;

    float heldTime;
    [SerializeField] bool isHolding;

    private void OnEnable()
    {
        worldButton.enabled = false;
    }
    private void OnDisable()
    {
        worldButton.enabled = true;
    }

    private void Update()
    {
        if (isHolding)
        {
            heldTime = Mathf.Clamp(heldTime + Time.deltaTime, 0, slider.maxValue);

            slider.value = heldTime;

            if (heldTime >= slider.maxValue)
            {
                heldTime = 0;
                isHolding = false;
                menuGameManager.DeleteWorld(realWorldName);
            }
        }
        else if (heldTime > 0)
        {
            heldTime = Mathf.Clamp01(heldTime - Time.deltaTime);

            slider.value = heldTime;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isHolding = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isHolding = false;
    }

    internal void AddMenuGameManager(MenuGameManager menuGameManager, string realWorldName)
    {
        this.menuGameManager = menuGameManager;
        worldButton = transform.parent.GetComponent<Button>();
        this.realWorldName = realWorldName;
        slider = transform.GetComponent<Slider>();
    }
}