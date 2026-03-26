using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUIManager : MonoBehaviour
{
    [Header("Render")]
    public Slider renderDistanceSlider;
    public TextMeshProUGUI renderDistanceText;

    [Header("Music")]
    public Slider masterSlider;
    public TextMeshProUGUI masterText;
    public Slider musicSlider; // new
    public TextMeshProUGUI musicText;
    public Slider playerSlider; // new
    public TextMeshProUGUI playerText;

    [Header("Input")]
    public Slider sensitivitySlider;
    public TextMeshProUGUI sensitivityText;

    private void OnEnable()
    {
        OptionsFileHandler.Load();

        renderDistanceSlider.value = OptionsSettings.displayRenderDistance;
        renderDistanceText.text = OptionsSettings.displayRenderDistance.ToString();

        masterSlider.value = OptionsSettings.masterVolume * 10f;
        masterText.text = $"{OptionsSettings.masterVolume:F1}";
        musicSlider.value = OptionsSettings.musicVolume * 10f;
        musicText.text = $"{OptionsSettings.musicVolume:F1}";
        playerSlider.value = OptionsSettings.playerVolume * 10f;
        playerText.text = $"{OptionsSettings.playerVolume:F1}";

        sensitivitySlider.value = OptionsSettings.sensitivity / 2f;
        sensitivityText.text = $"{OptionsSettings.sensitivity / 20f:F1}";
    }

    private void OnDisable()
    {
        OptionsFileHandler.Save(
            (int)renderDistanceSlider.value,
            masterSlider.value / 10f,
            musicSlider.value / 10f,
            playerSlider.value / 10f,
            sensitivitySlider.value * 2f
        );

        OptionsSettings.SaveOptions((int)renderDistanceSlider.value, masterSlider.value / 10f, musicSlider.value / 10f, playerSlider.value / 10f, sensitivitySlider.value * 2f);
    }

    // In editor
    public void ChangeSlidersText()
    {
        renderDistanceText.text = $"{renderDistanceSlider.value}";

        masterText.text = $"{masterSlider.value / 10f:F1}";
        musicText.text = $"{musicSlider.value / 10f:F1}";
        playerText.text = $"{playerSlider.value / 10f:F1}";

        sensitivityText.text = $"{sensitivitySlider.value / 10f:F1}";
    }
    // ----
}