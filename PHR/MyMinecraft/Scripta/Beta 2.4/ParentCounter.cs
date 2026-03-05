using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class ParentCounter : MonoBehaviour
{
    [SerializeField] UnityEvent Run;
    [SerializeField] UnityEvent Cancel;
    
    int activeChildCount;
    bool alreadyRun, alreadyCanceled;

    private void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);

            child.gameObject.SetActive(false);
            child.AddComponent<ChildSpeaker>();
        }
    }

    internal void Add()
    {
        activeChildCount ++;
        if (activeChildCount > 0 && !alreadyRun)
        {
            Run.Invoke();

            alreadyRun = true;
            alreadyCanceled = false;
        }
    }
    internal void Remove()
    {
        activeChildCount --;
        if (activeChildCount <= 0 && !alreadyCanceled)
        {
            Cancel.Invoke();

            alreadyRun = false;
            alreadyCanceled = true;
        }
    }

    private void OnTransformChildrenChanged()
    {
        transform.GetChild(transform.childCount - 1).AddComponent<ChildSpeaker>();
    }

    internal void AddPlayerEvents()
    {
        Run.AddListener(() => PlayerSettings.playerCamera.enabled = false);
        Cancel.AddListener(() => PlayerSettings.playerCamera.enabled = true);

        Run.AddListener(() => PlayerSettings.playerInteractions.ChangeInMenu(true));
        Cancel.AddListener(() => PlayerSettings.playerInteractions.ChangeInMenu(false));
    }

    internal void RemovePlayerEvents()
    {
        Run.RemoveListener(() => PlayerSettings.playerCamera.enabled = false);
        Cancel.RemoveListener(() => PlayerSettings.playerCamera.enabled = true);
    }

    internal int GetActiveCount()
    {
        return activeChildCount;
    }
}