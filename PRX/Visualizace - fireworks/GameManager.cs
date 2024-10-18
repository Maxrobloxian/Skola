//////////////////////////////////////////////////////////
// Kod se stara o vypnuti hry a vytvareni vybuhu na click
//////////////////////////////////////////////////////////

using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject boom;
    void Update()
    {
        // Vypnuti hry
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        // Vybuch na click
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Instantiate(boom, mousePos, Quaternion.identity);
        }
    }
}
