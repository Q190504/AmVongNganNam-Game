using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager _instance;
    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource gameSongSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Audio Setting")]

    [Header("Audio Setting Panel")]
    [SerializeField] private GameObject audioSettingsPanel;

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
}
