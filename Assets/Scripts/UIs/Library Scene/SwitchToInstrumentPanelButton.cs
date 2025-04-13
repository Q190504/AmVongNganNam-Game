using UnityEngine;

public class SwitchToInstrumentPanelButton : BaseButton
{
    [SerializeField] private VoidPublisherSO switchToInstrumentPanelSO;

    public void SwitchToInstrumentPanel()
    {
        ButtonClick();
        switchToInstrumentPanelSO.RaiseEvent();
    }
}
