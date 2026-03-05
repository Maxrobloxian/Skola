using UnityEngine;
using UnityEngine.UI;

public class UIMenu : MonoBehaviour
{
    Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    // In editor
    public void ChangeBackground(bool activeChildren)
    {
        if (activeChildren) image.enabled = true;
        else image.enabled = false;
    }
}