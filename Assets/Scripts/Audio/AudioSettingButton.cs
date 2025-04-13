using UnityEngine;

public class AudioSettingButton : BaseButton
{
    [SerializeField] private IntPublisherSO openAudioSettingsPanelSO;

    public void OpenAudioSettingsPanel()
    {
        ButtonClick();
        openAudioSettingsPanelSO.RaiseEvent(1);
    }
}
