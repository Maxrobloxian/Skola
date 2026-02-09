using UnityEngine;

public class GameManager : MonoBehaviour
{
#if DEBUG
    private void Awake()
    {
        gameObject.AddComponent<GameStats>();
    }
#endif
}