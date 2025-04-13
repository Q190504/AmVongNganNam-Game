using UnityEngine;

public class BaseButton : MonoBehaviour
{
    [SerializeField] private VoidPublisherSO buttonClickPublisher;
    [SerializeField] private VoidPublisherSO switchSceneButtonClickPublisher;

    public void ButtonClick()
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
