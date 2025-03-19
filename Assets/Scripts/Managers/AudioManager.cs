using TheHeroesJourney;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager _instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource gameSongSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Audio Clips")]

    [Header("BGMs")]
    [SerializeField] AudioClip bgm;

    [Header("SFXs")]
    [SerializeField] AudioClip buttonClick;
    [SerializeField] AudioClip playButtonClick;

    [Header("Audio Settings Panel")]
    private AudioSettingsPanel audioSettingsPanel;
    private CanvasGroup audioSettingsPanelCanvasGroup;


    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindFirstObjectByType<AudioManager>();
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
            Debug.Log("Found more than one Audio Manager in the scene. Destroying the newest one");
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        PlayBGM(bgm);
    }

    public void PlayBGM(AudioClip clip)
    {
        bgmSource.clip = clip;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    public void PlayGameSong(AudioClip clip)
    {
        if (clip == null) return;
        gameSongSource.clip = clip;
        gameSongSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip);
    }

    public void PauseGameSong()
    {
        gameSongSource.Pause();
    }

    public void ContinueGameSong()
    {
        gameSongSource.Play();
    }

    public void StopGameSong()
    {
        gameSongSource.Stop();
    }

    public float GetGameSongSecond()
    {
        return gameSongSource.time;
    }

    public void SetBGMVolume(float volume) => bgmSource.volume = volume;
    public void SetGameSongVolume(float volume) => gameSongSource.volume = volume;
    public void SetSFXVolume(float volume) => sfxSource.volume = volume;


    public void PlayClickButtonSFX()
    {
        PlaySFX(buttonClick);
    }

    public void PlayButtonPlayClickSFX()
    {
        PlaySFX(playButtonClick);
    }

    public void ToggleAudioSettingsPanel(int alpha)
    {
        audioSettingsPanel = FindFirstObjectByType<AudioSettingsPanel>();
        audioSettingsPanelCanvasGroup = audioSettingsPanel.GetComponent<CanvasGroup>();

        if (alpha == 0)//closed
        {
            audioSettingsPanelCanvasGroup.blocksRaycasts = false;
            audioSettingsPanelCanvasGroup.interactable = false;
        }
        else//opened
        {
            audioSettingsPanelCanvasGroup.blocksRaycasts = true;
            audioSettingsPanelCanvasGroup.interactable = true;

            audioSettingsPanel.SetSFXSlider();
            audioSettingsPanel.SetMusicSlider();
        }

        audioSettingsPanelCanvasGroup.alpha = alpha;
    }

    public void ExitGameMode()
    {
        StopGameSong();
        PlayBGM(bgm);
    }

    public void ToggleGameSong()
    {
        if (PauseManager.IsPause)
            PauseGameSong();
        else
            ContinueGameSong();
    }
}
