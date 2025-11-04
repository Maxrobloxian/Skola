using UnityEngine;

public class Target : MonoBehaviour
{
    // Kod terce

    [SerializeField] private float OriginalHP = 1;
    [SerializeField] private float HP;
    [SerializeField] private UIManager UIManager;
    [SerializeField] private ShootingPadManager shootingPadManager;
    [SerializeField] private GameObject targetGO;
    public Target cloneTarget;
    private bool inPosition;
    [SerializeField] private WorldUIManager worldUIManager;

    // Najde objekt Target a udela float HP stejny jako OriginalHP
    private void Start()
    {
        targetGO = GameObject.Find("Target");
        HP = OriginalHP;
    }

    // Kontroluje jestli je clovek na sve posici a posila informace o zivotu terce do jineho kodu
    private void Update()
    {
        inPosition = shootingPadManager.inPosition;
        worldUIManager.originalHPValue = OriginalHP;
        worldUIManager.actualHPValue = cloneTarget.HP;
    }

    // Dostava informace o sile utoku pokud je terc trefenej
    public void Hit(float receavedDamage)
    {
        // Pokud je clovek v posici vezme ze zivotu tolik kolik je sila utoku, pokud ne tak ukaze text s pismem "Out of range"
        if (inPosition)
        {
            HP -= receavedDamage;

            // Pokud je zivot mensi nebo stejni s 0, tak vitvori 2 inty s random hodnotou mezi {-7; 7} a {0; 5}, vytvori clonu terce ktery se bude posouvat po informaci z intu, vytvori clon kodu se jmenem cloneTarget, znici trefenej terc a zvetsi score o 1 bod
            if (HP <= 0)
            {
                int randomRangeX = Random.Range(-7, 8);
                int randomRangeY = Random.Range(0, 6);
                GameObject clone = Instantiate(targetGO, new Vector3(randomRangeX, randomRangeY, 8), Quaternion.Euler(0, 0, 0));
                cloneTarget = clone.GetComponent<Target>();
                Destroy(gameObject);
                UIManager.ManageScore(1);
            }
        }
        else { UIManager.AddNotInPosition(); }
    }

    // Dostava informace o novim floatu ktery bude menit zivot terce
    public void ChangeHP(float newHP)
    {
        OriginalHP = newHP;
        HP = newHP;
    }

    // Vitvori 2 inty s random hodnotou mezi {-7; 7} a {0; 5}, vytvori clonu terce ktery se bude posouvat po informaci z intu
    public void StartGame()
    {
        int randomRangeX = Random.Range(-7, 8);
        int randomRangeY = Random.Range(0, 6);
        Instantiate(targetGO, new Vector3(randomRangeX, randomRangeY, 8), Quaternion.Euler(0, 0, 0));
    }
}