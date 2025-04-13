using UnityEngine;

public class SwitchToSongPanelButton : BaseButton
{
    [SerializeField] private VoidPublisherSO switchToSongPanelSO;

    public void SwitchToSongPanel()
    {
        ButtonClick();
        switchToSongPanelSO.RaiseEvent();
    }
}
