using UnityEngine;
using UnityEngine.EventSystems;

public class UIPlayAudioEnter : MonoBehaviour
{
    [SerializeField] string onEnterSound;

    private void Start()
    {
        if (!TryGetComponent(out EventTrigger trigger))
            trigger = gameObject.AddComponent(typeof(EventTrigger)) as EventTrigger;

        EventTrigger.Entry entry = new() { eventID = EventTriggerType.PointerEnter };
        entry.callback.AddListener((eventData) => { OnEnter(); });
        trigger.triggers.Add(entry);
    }
    private void OnEnter()
    {
        SFXManager.Instance.PlayOneShot(onEnterSound);
    }
}