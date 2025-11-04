using UnityEngine;

public class TargetManager1 : MonoBehaviour
{
    // Propojuju veci mezi sebou
    [SerializeField] private GameObject targetGO;
    [SerializeField] private GameObject shootingPad;
    [SerializeField] private ShootingPadManager shootingPadManager;
    [SerializeField] private GameObject startGameButton;
    [SerializeField] private UIManager UIManager;
    [SerializeField] private TargetManager targetManager;

    // Na zacatku hry je kod neaktivni
    private void Start()
    {
        gameObject.SetActive(false);
    }

    // Zastavuje terc
    public void Hit()
    {
        targetGO = GameObject.Find("Target(Clone)");
        Destroy(targetGO);
        startGameButton.SetActive(true);
        shootingPadManager.inPosition = false;
        shootingPad.SetActive(false);
        UIManager.sec = 0;
        targetManager.CancelInvoke();
        gameObject.SetActive(false);
    }
}