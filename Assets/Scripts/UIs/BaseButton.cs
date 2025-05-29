using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(EventTrigger))]
public class BaseButton : MonoBehaviour
{
    [SerializeField] private VoidPublisherSO buttonClickPublisher;
    [SerializeField] private VoidPublisherSO switchSceneButtonClickPublisher;

    private void Start()
    {
        AddPointerEnterEvent();
    }

    private void AddPointerEnterEvent()
    {
        EventTrigger trigger = GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = gameObject.AddComponent<EventTrigger>();
        }

        EventTrigger.Entry entry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerEnter
        };
        entry.callback.AddListener((data) => { ButtonEnter(); });

        trigger.triggers.Add(entry);
    }


    public void ButtonClick()
    {
        if (buttonClickPublisher == null) return;
        buttonClickPublisher.RaiseEvent();
    }

    public void ButtonEnter()
    {
        if (buttonClickPublisher == null) return;
        buttonClickPublisher.RaiseEvent();
    }

    public void SwitchSceneButtonClick()
    {
        if (switchSceneButtonClickPublisher == null) return;
        switchSceneButtonClickPublisher.RaiseEvent();
    }
}
