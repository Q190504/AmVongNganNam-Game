using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Unity.VisualScripting.Member;

public class SongPlayer : MonoBehaviour
{
    [SerializeField] private SongInfoSO song;

    [Header("UI References")]
    [SerializeField] private Button playSongButton;
    [SerializeField] private Image diskImage;
    [SerializeField] private Slider songSlider;
    [SerializeField] private TextMeshProUGUI currentTimeText;
    [SerializeField] private TextMeshProUGUI totalTimeText;

    [SerializeField] private Sprite playSongButtonSprite;
    [SerializeField] private Sprite playingSongButtonSprite;
    [SerializeField] private Sprite continueSongButtonSprite;
    [SerializeField] private Sprite loadingSongButtonSprite;

    private bool isDraggingSlider = false;

    public void SetSong(SongInfoSO song)
    {
        this.song = song;
        
        AudioManager.Instance.StopGameSong();
        AudioManager.Instance.ClearGameSongClip();

        if (!AudioManager.Instance.IsBGMPlaying())
        {
            AudioManager.Instance.PlayBGM();
        }

        songSlider.value = 0;

        if (currentTimeText != null)
            currentTimeText.text = FormatTime(0);

        playSongButton.GetComponent<Image>().sprite = playSongButtonSprite;
    }

    public void HandleButtonClick()
    {
        AudioSource source = AudioManager.Instance.GetGameSongSource();

        if (source == null || song == null)
        {
            playSongButton.GetComponent<Image>().sprite = playSongButtonSprite;
            return;
        }

        if (!source.isPlaying)
        {
            if (source.clip == song.songClip && source.time > 0f)
            {
                ContinueSong();
            }
            else
            {
                PlaySong();
            }
        }
        else
        {
            playSongButton.GetComponent<Image>().sprite = continueSongButtonSprite;
            PauseSong();
        }
    }

    public void PauseSong()
    {
        AudioManager.Instance.PauseGameSong();

    }
    public void ContinueSong()
    {
        AudioManager.Instance.ContinueGameSong();
        StartCoroutine(UpdateSlider());

    }

    public void PlaySong()
    {
        StartCoroutine(PlaySongClip());
    }

    private IEnumerator PlaySongClip()
    {
        if (song != null)
        {
            if (song.songClip == null)
            {
                bool isDownloadComplete = false;
                SongLoader.Instance.DownloadSong(song, () => isDownloadComplete = true);
                playSongButton.GetComponent<Image>().sprite = loadingSongButtonSprite;
                yield return new WaitUntil(() => isDownloadComplete);
            }

            if (song.songClip)
            {
                AudioManager.Instance.StopBGM();
                AudioManager.Instance.PlayGameSong(song.songClip);
                StartCoroutine(UpdateSlider());
            }
        }
    }

    private IEnumerator UpdateSlider()
    {
        AudioSource source = AudioManager.Instance.GetGameSongSource();
        if (source == null || song.songClip == null)
            yield break;

        songSlider.maxValue = song.songClip.length;

        if (totalTimeText != null)
            totalTimeText.text = FormatTime(song.songClip.length);

        while (source != null && source.isPlaying)
        {
            if (!isDraggingSlider)
            {
                songSlider.value = source.time;

                if (currentTimeText != null)
                    currentTimeText.text = FormatTime(source.time);
            }

            if (diskImage != null)
            {
                diskImage.transform.Rotate(Vector3.forward * 100f * Time.deltaTime);
            }

            playSongButton.GetComponent<Image>().sprite = playingSongButtonSprite;
            yield return null;
        }
    }

    public void OnSliderValueChanged(float value)
    {
        if (isDraggingSlider && AudioManager.Instance.GetGameSongSource() != null)
        {
            AudioManager.Instance.GetGameSongSource().time = value;
            if (currentTimeText != null)
                currentTimeText.text = FormatTime(value);
        }
    }

    public void OnSliderPointerDown()
    {
        isDraggingSlider = true;
        if (AudioManager.Instance.GetGameSongSource() != null)
        {
            float value = songSlider.value;
            AudioManager.Instance.GetGameSongSource().time = value;

            if (currentTimeText != null)
                currentTimeText.text = FormatTime(value);
        }
    }

    public void OnSliderPointerUp()
    {
        isDraggingSlider = false;
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        return $"{minutes:D2}:{seconds:D2}";
    }

    private void OnDestroy()
    {
        AudioManager.Instance.StopGameSong();
    }

}
