using UnityEngine;

public class AudioSettingButton : MonoBehaviour
{
    [SerializeField] private IntPublisherSO openAudioSettingsPanelSO;

    public void OpenAudioSettingsPanel()
    {
        openAudioSettingsPanelSO.RaiseEvent(1);
    }
}
