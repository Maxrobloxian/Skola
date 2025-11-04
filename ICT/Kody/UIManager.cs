using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    //Spousta floatu, textu a par slideru, boolu, objectu a propojenych kodu
    [SerializeField] private float amoCount;
    [SerializeField] private float amoSize;
    [SerializeField] private float score;
    public float sec;
    [SerializeField] private TextMeshProUGUI amoText;
    [SerializeField] private TextMeshProUGUI reloadText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI shootCountdownText;
    [SerializeField] private TextMeshProUGUI notInPositionText;
    [SerializeField] private Slider reloadBar;
    private bool reloading;
    private GameObject hand;
    private WeaponUser weaponUser;

    // Hleda ruku a kod zbrani ktera v ni je taky jestli je clovek na sve positi
    void Start()
    {
        hand = GameObject.Find("Hand");
        weaponUser = hand.GetComponent<WeaponUser>();

        notInPositionText.enabled = false;
    }

    void Update()
    {
        // Bere floaty z jinych kodu a prepisuje je sem
        amoCount = weaponUser.amo;
        amoSize = weaponUser.reservoirSize;
        reloading = weaponUser.reloading;
        
        // Zapina a vzpina text ukazujici pocet kulec ve zbrani
        if (amoSize <= 0) { amoText.enabled = false; }
        else { amoText.enabled = true; }

        // ukazuje text pokud zbrani nema kulky
        if (amoCount <= 0 && amoSize > 0 && !reloading) { reloadText.enabled = true; }
        else if (reloading)// <------ ukazyje animaci naplneni naboju pokud se zbrani nabii
        {
            reloadBar.enabled = true;
            reloadBar.value += Time.deltaTime;
            reloadText.enabled = true;
        }
        else// <----- zavira animaci a text pokud a zbrani nabyta
        {
            reloadBar.value = 0;
            reloadBar.enabled = false;
            reloadText.enabled = false;
        }

        // Ukazuje odpocitavani pro zacatek hry pokud je float sec vetsi nez 0
        if (sec > 0) {
            sec -= Time.deltaTime;
            shootCountdownText.enabled = true;
            shootCountdownText.text = sec.ToString("0");
            if (sec <= .5f) { shootCountdownText.text = "Go!"; }
        }
        else { shootCountdownText.enabled = false; }

        // Ukazuje maximalni a aktualni pocet kulek ve zbrani
        amoText.text = "Amo: " + amoCount + "/" + amoSize;
    }

    // Dodava informace pro odpocitani zacatku hry
    public void ManageScore(float scoreAmount)
    {
        score += scoreAmount;
        scoreText.text = "Score: " + score;
    }

    // Pokud clovek neni v poziti ukazyje text po dobu .5 sekund
    public void AddNotInPosition()
    {
        notInPositionText.enabled = true;
        Invoke(nameof(RemoveNotInPosition), .5f);
    }

    private void RemoveNotInPosition()
    {
        notInPositionText.enabled = false;
    }
}