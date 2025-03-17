using UnityEngine;

public class CloseAudioSettingsPanelButton : MonoBehaviour
{
    public IntPublisherSO closeAudioSettingsPanelSO;

    public void CloseAudioSettingsPanel()
    {
        closeAudioSettingsPanelSO.RaiseEvent(0);
    }
}
