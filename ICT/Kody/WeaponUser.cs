using UnityEngine;

public class WeaponUser : MonoBehaviour
{
    //Kod prijimaci informace a pouziti zbrane

    [Header("Weapon Options")]
    [SerializeField] private float damage;
    public float reservoirSize;
    public float amo;
    [SerializeField] private float fireSpeed;
    [SerializeField] private bool automatic;

    [SerializeField] private Animator shootAnim;

    [Header("Transform")]
    [SerializeField] private Transform playerCamera;

    private float fireCooldown;

    private bool isEquipped = false;
    private bool needReload = false;
    public bool reloading = false;

    void Update()
    {
        //Automatic
        if (Input.GetKey(KeyCode.Mouse0) && isEquipped && automatic && !needReload && !reloading)
        {
            if (fireCooldown <= 0f)
            {
                if (shootAnim != null) { shootAnim.SetTrigger("Shoot"); }
                Shoot();
                fireCooldown = fireSpeed;
                amo -= 1;
                Debug.Log("Fire");
            }
        }
        //Manual
        else if (Input.GetKeyDown(KeyCode.Mouse0) && isEquipped && !automatic && !needReload && !reloading)
        {
            if (fireCooldown <= 0f)
            {
                if (shootAnim != null) { shootAnim.SetTrigger("Shoot"); }
                Shoot();
                fireCooldown = fireSpeed;
                amo -= 1;
                Debug.Log("Fire");
            }
        }

        //Reload
        if (Input.GetKeyDown(KeyCode.R) && !reloading && isEquipped|| Input.GetKeyDown(KeyCode.Mouse0) && !reloading && needReload && isEquipped)
        {
            Debug.Log("Reloading");
            Invoke(nameof(Reload), 1);
            reloading = true;
        }
    
        //Drop
        if (Input.GetKeyDown(KeyCode.Q) && isEquipped)
        {
            DropWeapon();
        }

        if (fireCooldown > 0) { fireCooldown -= Time.deltaTime; }

        if (amo <= 0) { needReload = true; }
        else { needReload = false; }
    }

    // Pokud hrac trefi jednu z urcenich objectu tak posle signal
    private void Shoot()
    {
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out RaycastHit hit))
        {
            Target target = hit.transform.GetComponent<Target>();// <------ tenhle kod dostava kod od objectu
            if (target != null)
            {
                target.Hit(damage);// <----- kod posila informace o tom ze bul dany object trefenej
            }
            TargetManager targetManager = hit.transform.GetComponent<TargetManager>();
            if (targetManager != null)
            {
                targetManager.Hit();
            }
            TargetManager1 targetManager1 = hit.transform.GetComponent<TargetManager1>();
            if (targetManager1 != null)
            {
                targetManager1.Hit();
            }
            THPLvL thplvl = hit.transform.GetComponent<THPLvL>();
            if (thplvl != null)
            {
                thplvl.Hit();
            }
        }
    }

    // Nabije zbran kulkama, ukazuje ze se zbran uz nenabiji a posila informace jinemu kodu ze se zbran nabyla
    private void Reload()
    {
        amo = reservoirSize;
        reloading = false;
        WeaponManager weaponManager2 = GetComponentInChildren<WeaponManager>();
        weaponManager2.WeaponReloaded();
    }

    // Dostava informace o zbrani (silu, pocet kulek, ...) a vypina dale nedulezity kod
    public void GetWeaponData()
    {
        isEquipped = true;
        WeaponManager weaponManager = GetComponentInChildren<WeaponManager>();
        damage = weaponManager.damage;
        reservoirSize = weaponManager.reservoirSize;
        fireSpeed = 1/weaponManager.fireSpeed;
        automatic = weaponManager.automatic;
        shootAnim = weaponManager.shootAnim;
        weaponManager.enabled = false;

        fireCooldown = fireSpeed;
        amo = reservoirSize;
    }

    // Maze informace o zbrani a zapina kod
    private void DropWeapon()
    {
        isEquipped = false;
        shootAnim = null;
        WeaponManager weaponManager1 = GetComponentInChildren<WeaponManager>();
        damage = 0;
        reservoirSize = 0;
        amo = 0;
        fireSpeed = 0;
        weaponManager1.enabled = true;
        weaponManager1.WeaponDropped();
    }
}