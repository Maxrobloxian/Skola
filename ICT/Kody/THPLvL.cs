using UnityEngine;

public class THPLvL : MonoBehaviour
{
    // Urcije LvL a zivot hry

    // Hromada propojenych veci, floatu, boolu a kodu
    [SerializeField] private float newHP;
    [SerializeField] private Target target;
    [SerializeField] private GameObject level1;
    [SerializeField] private GameObject level2;
    [SerializeField] private GameObject level3;
    [SerializeField] private bool ignore1;
    [SerializeField] private bool ignore2;
    [SerializeField] private bool ignore3;
    [SerializeField] private TargetManager1 targetManager1;

    // Zavre jeden z levelu a posila signal voidu StartDebug
    private void Start()
    {
        ignore1 = true;
        level1.SetActive(false);
        StartDebug();
    }

    // Ujistuje to ze jsou vzdi 2 levely otevreny
    private void Update()
    {
        if (!level1.activeSelf && !ignore1)
        {
            ignore1 = true;
            ignore2 = false;
            ignore3 = false;
            level2.SetActive(true);
            level3.SetActive(true);
        }
        else if (!level2.activeSelf && !ignore2)
        {
            ignore1 = false;
            ignore2 = true;
            ignore3 = false;
            level1.SetActive(true);
            level3.SetActive(true);
        }
        else if (!level3.activeSelf && !ignore3)
        {
            ignore1 = false;
            ignore2 = false;
            ignore3 = true;
            level1.SetActive(true);
            level2.SetActive(true);
        }
    }

    // Pokud strelim na object tak meni level obtiznosti
    public void Hit()
    {
        target = target.cloneTarget;
        target.ChangeHP(newHP);
        Debug.Log("Changed HP");// <----- pyse ze se zmenil zivot terce
        gameObject.SetActive(false);
        targetManager1.Hit();// <----- posila informace ze se ma zastavit hra
    }

    // To sami jako v Updatu, je to tu abuch odstranil bagy
    private void StartDebug()
    {
        if (!level1.activeSelf && !ignore1)
        {
            ignore1 = true;
            ignore2 = false;
            ignore3 = false;
            level2.SetActive(true);
            level3.SetActive(true);
        }
        else if (!level2.activeSelf && !ignore2)
        {
            ignore1 = false;
            ignore2 = true;
            ignore3 = false;
            level1.SetActive(true);
            level3.SetActive(true);
        }
        else if (!level3.activeSelf && !ignore3)
        {
            ignore1 = false;
            ignore2 = false;
            ignore3 = true;
            level1.SetActive(true);
            level2.SetActive(true);
        }
    }
}