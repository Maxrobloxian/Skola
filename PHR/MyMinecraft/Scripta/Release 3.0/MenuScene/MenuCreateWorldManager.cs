using System.IO;
using TMPro;
using UnityEngine;

public class MenuCreateWorldManager : MonoBehaviour
{
    [SerializeField] MenuGameManager menuGameManager;

    [Header("Inputs")]
    [SerializeField] TMP_InputField displayWorldNameInput;
    [SerializeField] TMP_InputField seedXInput, seedYInput;

    [Header("Text")]
    [SerializeField] TextMeshProUGUI displayWorldNameTMP;
    [SerializeField] TextMeshProUGUI realWorldNameTMP, seedXInputTextTMP, seedYInputTextTMP;

    string realWorldName;

    Color seedTextColor;
    Color displayNameTextColor;
    Color realNameTextColor;

    Color wrongTextColor = Color.red;

    private void Awake()
    {
        realWorldName = displayWorldNameInput.text;

        seedTextColor = seedXInputTextTMP.color;
        displayNameTextColor = displayWorldNameTMP.color;
        realNameTextColor = realWorldNameTMP.color;
    }

    private void OnEnable()
    {
        RandomizeSeed();
        seedXInputTextTMP.color = seedTextColor * new Color(1f, 1f, 1f, .5f);
        seedYInputTextTMP.color = seedTextColor * new Color(1f, 1f, 1f, .5f);

        displayWorldNameInput.text = "New World";
        ChangeRealNameText();
        displayWorldNameTMP.color = new Color(0.1960784f, 0.1960784f, 0.1960784f, .5f);
    }

    // In editor
    public void StartWorldButton()
    {
        if (CheckSeedValues() && CheckNamesValues())
            if (!WorldFileHandler.CreateNew(realWorldName, displayWorldNameInput.text, new Vector2Int(int.Parse(seedXInput.text), int.Parse(seedYInput.text)))) return;

        menuGameManager.StartWorld(realWorldName);
    }

    public void RandomizeSeed()
    {
        seedXInput.text = Random.Range(-50000, 50000).ToString();
        seedYInput.text = Random.Range(-50000, 50000).ToString();
    }

    public void ChangeRealNameText()
    {
        ChangeRealNameLooped(displayWorldNameInput.text);
    }

    public void CheckSeedInput(bool indexX)
    {
        CheckSeedValue(indexX);
    }
    // ----

    void ChangeRealNameLooped(string worldDisplayName)
    {
        worldDisplayName = WorldFileHandler.DisplayToRealName(worldDisplayName);

        if (CheckNamesValues() && Directory.Exists($"{Application.streamingAssetsPath}/Worlds/{worldDisplayName}"))
        {
            int loopNr = 1;
            while (Directory.Exists($"{Application.streamingAssetsPath}/Worlds/{worldDisplayName} ({loopNr})"))
            {
                loopNr += 1;
            }

            realWorldName = $"{worldDisplayName} ({loopNr})";
        }
        else realWorldName = worldDisplayName;

        realWorldNameTMP.text = $"Folder Name: {realWorldName}";

        CheckNamesValues();
    }

    void ChangeNamesColor(bool isDisplay, Color color)
    {
        if (isDisplay) displayWorldNameTMP.color = color;
        else realWorldNameTMP.color = color;
    }

    bool CheckSeedValues()
    {
        return CheckSeedValue(true) && CheckSeedValue(false);
    }

    bool CheckSeedValue(bool valueX)
    {
        if (valueX)
        {
            if (!SeedInBounds(seedXInput.text))
            {
                seedXInputTextTMP.color = wrongTextColor;
                return false;
            }
            seedXInputTextTMP.color = seedTextColor;
        }
        else
        {
            if (!SeedInBounds(seedYInput.text))
            {
                seedYInputTextTMP.color = wrongTextColor;
                return false;
            }
            seedYInputTextTMP.color = seedTextColor;
        }

        return true;
    }

    bool SeedInBounds(string seedText)
    {
        if (string.IsNullOrEmpty(seedText) || seedText == "-" || int.Parse(seedText) < -50000 || int.Parse(seedText) > 50000) return false;

        return true;
    }

    bool CheckNamesValues()
    {
        bool isFine = true;

        if (string.IsNullOrWhiteSpace(displayWorldNameInput.text))
        {
            print("Change color to red -> display");
            ChangeNamesColor(true, wrongTextColor);
            isFine = false;
        }
        else ChangeNamesColor(true, displayNameTextColor);

        if (string.IsNullOrWhiteSpace(realWorldName))
        {
            print("Change color to red -> real");
            ChangeNamesColor(false, wrongTextColor);
            isFine = false;
        }
        else ChangeNamesColor(false, realNameTextColor);

        return isFine;
    }
}