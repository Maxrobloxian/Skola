using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    // Ruzne floaty
    public float damage;
    public float reservoirSize;
    public float fireSpeed;
    public bool automatic;

    // Propojuju animaci s kodem
    public Animator shootAnim;

    // propojuju ruku rigidboty a generuju bool
    private GameObject hand;

    bool isEquipped = false;

    Rigidbody rb;

    void Start()
    {
        // Davam hodnotu pro rigidbody a animaci taky hledam poziti hand
        rb = GetComponent<Rigidbody>();
        hand = GameObject.Find("Hand");
        shootAnim = GetComponent<Animator>();

        // davam rychlost manualnich a neupravenych zbrani
        if (fireSpeed == 0 || !automatic)
        {
            fireSpeed = 10f;
        }
    }

    // Kdy je clovek blysko zbrani tak zacina kod uvnitr voidu
    private void OnTriggerStay(Collider other)
    {
        // Pokud zmacknu klavesu E a nemam uz zbrani tak zacina kod
        if (other.transform.CompareTag("Player") && Input.GetKey(KeyCode.E) && !isEquipped)
        {
            isEquipped = true;
            rb.isKinematic = true;
            transform.SetParent(hand.transform);// <----- bere zbrani do ruky
            transform.localPosition = Vector3.zero;// <------ meni posici
            transform.localRotation = Quaternion.Euler(Vector3.zero);// <------ meni rotaci
            Physics.IgnoreLayerCollision(3, 6);// <------ zajistuje ze se zbrani ne zasekava v cloveku

            // Dostava a posila informace zbrane do a z jineho kodu
            WeaponUser weaponUser = hand.GetComponent<WeaponUser>();
            weaponUser.GetWeaponData();
        }
    }

    // Pokud dostane signal tak resetuje rotaci zbrane
    public void WeaponReloaded()
    {
        transform.localRotation = Quaternion.Euler(Vector3.zero);
    }

    // Pokud dostane signal hazi zbran na zem
    public void WeaponDropped()
    {
        isEquipped = false;
        rb.isKinematic = false;
        transform.SetParent(null);
        Physics.IgnoreLayerCollision(3, 6, false);

        rb.AddForce(hand.transform.forward * 16f, ForceMode.Impulse);
    }
}