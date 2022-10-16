using UnityEngine;
using UnityEngine.EventSystems;

public class UIPlayAudioClick : MonoBehaviour
{
    [SerializeField] string onClickSound;

    private void Start()
    {
        if (!TryGetComponent(out EventTrigger trigger))
            trigger = gameObject.AddComponent(typeof(EventTrigger)) as EventTrigger;

        EventTrigger.Entry entry = new() { eventID = EventTriggerType.PointerClick };
        entry.callback.AddListener((eventData) => { OnClick(); });
        trigger.triggers.Add(entry);
    }
    private void OnClick()
    {
        SFXManager.Instance.PlayOneShot(onClickSound);
    }
}