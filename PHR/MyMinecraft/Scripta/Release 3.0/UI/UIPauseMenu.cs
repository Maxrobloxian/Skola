using UnityEngine;

public class UIPauseMenu : MonoBehaviour
{
    [SerializeField] UIManager uiManager;

    private void OnEnable()
    {
        uiManager.InMenu(false);
    }
}