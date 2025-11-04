using UnityEngine;

public class TargetManager : MonoBehaviour
{
    [SerializeField] private Target target;
    [SerializeField] private GameObject shootingPad;
    [SerializeField] private UIManager UIManager;

    [SerializeField] private GameObject stopGameButton;


    // Deaktivuje misto kde se clovek ma dat do posice
    private void Start()
    {
        shootingPad.SetActive(false);
    }

    // Pokud nekdo streli do objectu posila informace pro text, vypina object a posila signal do voidu StartShootingGame po 3 sekundach
    public void Hit()
    {
        Invoke(nameof(StartShootingGame), 3);
        UIManager.sec = 3;
        stopGameButton.SetActive(true);
        gameObject.SetActive(false);
    }

    // Posila signal zacit hru do jineho kodu a aktivuje misto kde se clovek ma dat do posice
    private void StartShootingGame()
    {
        target.StartGame();
        shootingPad.SetActive(true);
    }
}