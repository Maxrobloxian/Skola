using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonTextHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] TextMeshProUGUI buttonText;

    Color normalColor;
    float hoverOffsetX, hoverOffsetY;

    private void Awake()
    {
        normalColor = buttonText.color;
        hoverOffsetX = buttonText.fontMaterial.GetFloat(ShaderUtilities.ID_UnderlayOffsetX);
        hoverOffsetY = buttonText.fontMaterial.GetFloat(ShaderUtilities.ID_UnderlayOffsetY);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonText.color = Color.white;
        buttonText.fontMaterial.SetFloat(ShaderUtilities.ID_UnderlayOffsetX, 0);
        buttonText.fontMaterial.SetFloat(ShaderUtilities.ID_UnderlayOffsetY, 0);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonText.color = normalColor;
        buttonText.fontMaterial.SetFloat(ShaderUtilities.ID_UnderlayOffsetX, hoverOffsetX);
        buttonText.fontMaterial.SetFloat(ShaderUtilities.ID_UnderlayOffsetY, hoverOffsetY);
    }
}