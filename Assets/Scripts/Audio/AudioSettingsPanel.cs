using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class AudioSettingsPanel : MonoBehaviour
{
    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("Sliders")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMusic()
    {
        float volume = musicSlider.value;
        audioMixer.SetFloat("music", Mathf.Log10(volume) * 20);
        //TO DO: SAVE
    }

    public void SetSFX()
    {
        float volume = sfxSlider.value;
        audioMixer.SetFloat("sfx", Mathf.Log10(volume) * 20);
        //TO DO: SAVE
    }

    public void SetSFXSlider()
    {
        audioMixer.GetFloat("sfx", out float sfxVolume);
        sfxSlider.value = Mathf.Pow(10, sfxVolume / 20);
    }

    public void SetMusicSlider()
    {
        audioMixer.GetFloat("music", out float musicVolume);
        musicSlider.value = Mathf.Pow(10, musicVolume / 20);
    }

    public AudioMixer GetAudioMixer()
    {
        return audioMixer;
    }
}
