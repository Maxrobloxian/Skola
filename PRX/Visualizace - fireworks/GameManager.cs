using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject boom;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Instantiate(boom, mousePos, Quaternion.identity);
        }
    }
}