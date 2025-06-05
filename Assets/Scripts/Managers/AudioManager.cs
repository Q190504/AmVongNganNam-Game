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

    [Header("BGMs")]
    [SerializeField] AudioClip bgm;

    [Header("SFXs")]
    [SerializeField] AudioClip buttonClickSFX;
    [SerializeField] AudioClip switchSceneButtonClickSFX;
    [SerializeField] AudioClip successSoundSFX;
    [SerializeField] AudioClip failureSoundSFX;
    [SerializeField] AudioClip unlockInstrumentSFX;
    [SerializeField] AudioClip hitSoundSFX;

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
            Debug.Log("[AudioManager] Found more than one instance. Destroying duplicate.");
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        PlayBGM(bgm);
    }

    public void PlayBGM()
    {
        PlayBGM(bgm);
    }

    public void PlayBGM(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("[AudioManager] BGM clip is null!");
            return;
        }

        Debug.Log($"[AudioManager] Playing BGM");
        bgmSource.clip = clip;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        Debug.Log("[AudioManager] Stopping BGM.");
        bgmSource.Stop();
    }

    public void PlayGameSong(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("[AudioManager] PlayGameSong called with NULL clip.");
            return;
        }

        Debug.Log($"[AudioManager] Playing game song, loop={gameSongSource.loop}, time={gameSongSource.time}");
        gameSongSource.loop = false;
        gameSongSource.clip = clip;
        gameSongSource.Play();
    }

    public void StopGameSong()
    {
        Debug.Log($"[AudioManager] Stopping game song at time: {gameSongSource.time}");
        gameSongSource.Stop();
    }

    public void PauseGameSong()
    {
        Debug.Log($"[AudioManager] Pausing game song at time: {gameSongSource.time}");
        gameSongSource.Pause();
    }

    public void ContinueGameSong()
    {
        Debug.Log($"[AudioManager] Continuing game song at time: {gameSongSource.time}");
        gameSongSource.Play();
    }

    public void RestartGame()
    {
        Debug.Log("[AudioManager] RestartGame() called.");
        StopGameSong();
        PlayGameSong(gameSongSource.clip);
    }

    public float GetGameSongSecond()
    {
        float currentTime = gameSongSource.time;
        return currentTime;
    }

    public float GetGameSongLength()
    {
        return gameSongSource.clip != null ? gameSongSource.clip.length : 0f;
    }

    public AudioSource GetGameSongSource()
    {
        return gameSongSource;
    }

    public void ClearGameSongClip()
    {
        Debug.Log("[AudioManager] Clearing game song clip.");
        gameSongSource.clip = null;
    }

    public bool IsBGMPlaying()
    {
        return bgmSource != null && bgmSource.isPlaying;
    }

    public void SetBGMVolume(float volume) => bgmSource.volume = volume;
    public void SetGameSongVolume(float volume) => gameSongSource.volume = volume;
    public void SetSFXVolume(float volume) => sfxSource.volume = volume;

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("[AudioManager] Tried to play null SFX.");
            return;
        }

        Debug.Log($"[AudioManager] Playing SFX: {clip.name}");
        sfxSource.PlayOneShot(clip);
    }

    public void PlayClickButtonSFX()
    {
        PlaySFX(buttonClickSFX);
    }

    public void PlayClickSwitchSceneButtonSFX()
    {
        PlaySFX(switchSceneButtonClickSFX);
    }

    public void PlayEndGameSoundSFX(bool result)
    {
        Debug.Log($"[AudioManager] Playing end game SFX. Result: {(result ? "Success" : "Failure")}");
        PlaySFX(result ? successSoundSFX : failureSoundSFX);
    }

    public void PlayUnlockSoundSFX()
    {
        PlaySFX(unlockInstrumentSFX);
    }

    public void PlayHitSoundSFX()
    {
        PlaySFX(hitSoundSFX);
    }

    public void ToggleAudioSettingsPanel(int alpha)
    {
        audioSettingsPanel = FindFirstObjectByType<AudioSettingsPanel>();
        audioSettingsPanelCanvasGroup = audioSettingsPanel?.GetComponent<CanvasGroup>();

        if (audioSettingsPanelCanvasGroup == null)
        {
            Debug.LogWarning("[AudioManager] Audio Settings Panel is missing or incomplete.");
            return;
        }

        bool isOpening = alpha != 0;
        Debug.Log($"[AudioManager] Toggling Audio Settings Panel. Opening: {isOpening}");

        audioSettingsPanelCanvasGroup.blocksRaycasts = isOpening;
        audioSettingsPanelCanvasGroup.interactable = isOpening;
        audioSettingsPanelCanvasGroup.alpha = alpha;

        if (isOpening)
        {
            audioSettingsPanel.SetSFXSlider();
            audioSettingsPanel.SetMusicSlider();
        }
    }

    public void ExitGameMode()
    {
        Debug.Log("[AudioManager] Exiting game mode.");
        StopGameSong();
        PlayBGM(bgm);
    }

    public void ToggleGameSong()
    {
        if (PauseManager.IsPause)
        {
            Debug.Log("[AudioManager] Game is paused. Pausing song.");
            PauseGameSong();
        }
        else
        {
            Debug.Log("[AudioManager] Game resumed. Continuing song.");
            ContinueGameSong();
        }
    }
}
