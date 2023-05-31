using UnityEngine;
using UnityEngine.EventSystems;

public class UIHoverChangeBongosColor : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] GameObject bongos;
    [SerializeField] Color onHoverEnter;

    private void Start()
    {
        if (!TryGetComponent(out EventTrigger trigger))
            trigger = gameObject.AddComponent(typeof(EventTrigger)) as EventTrigger;

        EventTrigger.Entry entry = new() { eventID = EventTriggerType.PointerEnter };
        entry.callback.AddListener((eventData) => { OnEnter(); });
        trigger.triggers.Add(entry);

        EventTrigger.Entry exit = new() { eventID = EventTriggerType.PointerExit };
        exit.callback.AddListener((eventData) => { OnExit(); });
        trigger.triggers.Add(exit);
    }
    private void OnEnter()
    {
        bongos.GetComponent<SpriteRenderer>().color = onHoverEnter;
        bongos.GetComponent<Animator>().speed = .25f;
    }

    private void OnExit()
    {
        bongos.GetComponent<SpriteRenderer>().color = Color.white;
        bongos.GetComponent<Animator>().speed = 1f;
    }

    public void OnClick()
    {
        bongos.GetComponent<Animator>().speed = 1f;
        bongos.GetComponent<Animator>().Play("BongoClick");
    }
}
