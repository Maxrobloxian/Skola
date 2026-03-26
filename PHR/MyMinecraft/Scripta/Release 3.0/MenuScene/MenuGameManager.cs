using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuGameManager : MonoBehaviour
{
    [SerializeField] GameObject titleScreen, optionsScreen, worldsScreen, createWorldScreen;
    [SerializeField] OptionsUIManager optionsUIManager;
    [SerializeField] GameObject worldsScreenWorldPrefab;
    [SerializeField] Transform worldsContent;

    [SerializeField] TextMeshProUGUI deleteWorldTMP;
    [SerializeField] Button backButton, createWorldButton;

    readonly string worldsPath = $"{Application.streamingAssetsPath}/Worlds/";
    readonly HashSet<string> loadedWorldsHash = new();

    bool isDeleting;

    private void Awake()
    {
        Time.timeScale = 1;
        OptionsFileHandler.Load();
    }

    // In editor
    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenOptionsScreen()
    {
        titleScreen.SetActive(false);
        optionsScreen.SetActive(true);
    }
    public void CLoseOptionsScreen()
    {
        titleScreen.SetActive(true);
        optionsScreen.SetActive(false);
    }

    public void OpenWorldsScreen()
    {
        AddWorldsToWorldsScreen();

        titleScreen.SetActive(false);
        worldsScreen.SetActive(true);
    }
    public void CloseWorldsScreen()
    {
        titleScreen.SetActive(true);
        worldsScreen.SetActive(false);
    }

    public void OpenCreateWorldScreen()
    {
        worldsScreen.SetActive(false);
        createWorldScreen.SetActive(true);
    }
    public void CloseCreateWorldScreen()
    {
        createWorldScreen.SetActive(false);
        worldsScreen.SetActive(true);
    }

    public void ToggleDeleteWorldScreen()
    {
        isDeleting = !isDeleting;
        foreach (Transform child in worldsContent)
        {
            child.GetChild(5).gameObject.SetActive(isDeleting);
        }
        deleteWorldTMP.text = isDeleting ? "Stop" : "Delete world";
        backButton.interactable = !isDeleting;
        createWorldButton.interactable = !isDeleting;
    }
    // ----

    async void MenuWorldAddData(Transform menuWorld, string worldPath, string displayWorldName, string realWorldName)
    {
        TextMeshProUGUI tempRealWorldName = menuWorld.GetChild(1).GetComponent<TextMeshProUGUI>();

        menuWorld.GetComponent<Button>().onClick.AddListener(() => StartWorld(tempRealWorldName.text));

        menuWorld.GetChild(0).GetComponent<TextMeshProUGUI>().text = displayWorldName;
        tempRealWorldName.text = realWorldName;

        menuWorld.GetChild(3).GetComponent<Button>().onClick.AddListener(() => ShowFile($"{Application.streamingAssetsPath}/Worlds/{realWorldName}"));
        menuWorld.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Calculating...";

        Transform sliderTransform = menuWorld.GetChild(5);
        sliderTransform.GetComponent<ButtonHoldManager>().AddMenuGameManager(this, realWorldName);
        sliderTransform.GetChild(2).GetComponent<TextMeshProUGUI>().text = displayWorldName;
        sliderTransform.GetChild(3).GetComponent<TextMeshProUGUI>().text = realWorldName;

        try
        {
            float? worldSize = await WorldFileHandler.GetWorldSize(worldPath);
            if (worldSize.HasValue) menuWorld.GetChild(4).GetComponent<TextMeshProUGUI>().text = $"{worldSize:F1} MB";
            else menuWorld.GetChild(4).GetComponent<TextMeshProUGUI>().text = "It gone?";
        } catch (System.Exception) { }
    }

    internal void StartWorld(string realWorldName)
    {
        if (WorldFileHandler.Load(realWorldName))
            SceneManager.LoadSceneAsync(1);
    }

    internal void DeleteWorld(string realWorldName)
    {
        WorldFileHandler.DeleteWorld(realWorldName);
        
        ResetWorldsToWorldsScreen();
    }

    void ShowFile(string realName)
    {
        System.Diagnostics.Process.Start("explorer.exe", "/select," + realName.Replace(@"/", @"\"));
    }

    void AddWorldsToWorldsScreen()
    {
        if (!Directory.Exists(worldsPath)) Directory.CreateDirectory(worldsPath);

        string[] worlds = Directory.GetDirectories(worldsPath);
        foreach (string worldPath in worlds)
        {
            string worldDisplayName = WorldFileHandler.GetWorldDisplayName(worldPath);
            
            if (string.IsNullOrEmpty(worldDisplayName) ||
                !loadedWorldsHash.Add(worldPath)) continue;

            MenuWorldAddData(Instantiate(worldsScreenWorldPrefab, worldsContent).transform, worldPath, worldDisplayName, Path.GetFileName(worldPath));
        }
    }

    void DeleteWorldsToWorldsScreen()
    {
        loadedWorldsHash.Clear();

        for (int i = worldsContent.childCount - 1; i >= 0; i--)
        {
            Destroy(worldsContent.GetChild(i).gameObject);
        }
    }

    void ResetWorldsToWorldsScreen()
    {
        DeleteWorldsToWorldsScreen();
        AddWorldsToWorldsScreen();
    }
}