using UnityEngine;

public class CloseAudioSettingsPanelButton : BaseButton
{
    public IntPublisherSO closeAudioSettingsPanelSO;

    public void CloseAudioSettingsPanel()
    {
        ButtonClick();
        closeAudioSettingsPanelSO.RaiseEvent(0);
    }
}
