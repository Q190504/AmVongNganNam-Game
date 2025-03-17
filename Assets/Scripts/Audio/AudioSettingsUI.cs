using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioSettingsUI : MonoBehaviour
{
    public static AudioSettingsUI _instance;

    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer musicAudioMixer;

    [Header("Sliders")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private Canvas currentCanvas;

    public static AudioSettingsUI Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindFirstObjectByType<AudioSettingsUI>();
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        SetMusic();
        SetSfx();

        FindCanvas(); 
        SceneManager.sceneLoaded += OnSceneLoaded; 
        gameObject.SetActive(false); 
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindCanvas();
    }

    private void FindCanvas()
    {
        Canvas newCanvas = FindFirstObjectByType<Canvas>();

        if (newCanvas != null && newCanvas != currentCanvas)
        {
            transform.SetParent(newCanvas.transform, false);
            currentCanvas = newCanvas;
        }
    }

    public void TogglePanel()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void SetMusic()
    {
        float volume = musicSlider.value;
        musicAudioMixer.SetFloat("music", Mathf.Log10(volume) * 20);
    }

    public void SetSfx()
    {
        float volume = sfxSlider.value;
        musicAudioMixer.SetFloat("sfx", Mathf.Log10(volume) * 20);
    }
}
