using UnityEngine;

public class ChildSpeaker : MonoBehaviour
{
    ParentCounter parentCounter;
    private void Awake()
    {
        parentCounter = transform.parent.GetComponent<ParentCounter>();
    }
    private void OnEnable()
    {
        parentCounter.Add();
    }
    private void OnDisable()
    {
        parentCounter.Remove();
    }
}