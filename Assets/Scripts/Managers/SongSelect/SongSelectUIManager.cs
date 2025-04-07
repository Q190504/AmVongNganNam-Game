using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SongSelectUIManager : MonoBehaviour
{
    [Header("Song")]
    public List<SongInfoSO> songList;
    public GameObject songButtonPrefab;
    public Transform songListContentPanel;
    public TMP_Text songNameText;
    public TMP_Text songInfoText;
    public TMP_Text songScoreText;
    public TMP_Text songStateText;

    [Header("UI")]
    public Button gameStartButton;
    public Button easyModeButton;
    public Button hardModeButton;
    public GameObject loadingPanel;
    public GameObject unlockConfirmPanel;
    public string gameSceneName;

    private List<GameObject> songButtonList = new List<GameObject>();
    private SongInfoSO currentSong;
    private GameManager.GameMode selectedGameMode;

    private bool? userConfirmedUnlock = null;

    private void Start()
    {
        loadingPanel.SetActive(false);

        songList = SongManager.Instance.GetSongInfos();
        var (selectedSong, savedMode) = SongManager.Instance.GetCurrentSelectedSong();
        selectedGameMode = savedMode;

        GenerateSongButtons();

        easyModeButton.onClick.AddListener(() => SetGameMode(GameManager.GameMode.NORMAL));
        hardModeButton.onClick.AddListener(() => SetGameMode(GameManager.GameMode.HARD));

        if (selectedSong == null)
        {
            var unlockedSongs = SongManager.Instance.GetGameData().unlocked_songs;
            string songToSelect = (unlockedSongs != null && unlockedSongs.Count > 0) ? unlockedSongs[0] : " ";
            SelectSong(songToSelect);
        }
        else
        {
            SelectSong(selectedSong.id);
        }
    }

    private void GenerateSongButtons()
    {
        for (int i = 0; i < songList.Count; i++)
        {
            int index = i; // capture index for lambda
            var song = songList[index];
            var songButton = Instantiate(songButtonPrefab, songListContentPanel);

            songButton.GetComponentInChildren<TextMeshProUGUI>().text = song.songName;
            songButtonList.Add(songButton);

            SetupSongButton(song, songButton);
        }
    }

    private void SetupSongButton(SongInfoSO song, GameObject songButton)
    {
        var data = SongManager.Instance.GetGameData();

        if (SongManager.Instance.IsSongIdInData(song.id))
        {
            UnlockSongUI(song, songButton);
        }
        else if (song.isDefault)
        {
            data.unlocked_songs.Add(song.id);
            UnlockSongUI(song, songButton);
        }
        else
        {
            songButton.GetComponent<Button>().onClick.AddListener(() => UnlockSong(song, songButton));
        }
    }

    private void UnlockSongUI(SongInfoSO song, GameObject songButton)
    {
        songButton.transform.Find("LockPanel").gameObject.SetActive(false);
        songButton.GetComponent<Button>().onClick.RemoveAllListeners();
        songButton.GetComponent<Button>().onClick.AddListener(() => SelectSong(song.id));
    }

    public void UnlockSong(SongInfoSO song, GameObject songButton)
    {
        StartCoroutine(HandleUnlockConfirmation(song, songButton));
    }

    private IEnumerator HandleUnlockConfirmation(SongInfoSO song, GameObject songButton)
    {
        userConfirmedUnlock = null;

        unlockConfirmPanel.SetActive(true);
        unlockConfirmPanel.GetComponentInChildren<TextMeshProUGUI>().text =
            $"You are unlocking {song.name} with {SongManager.Instance.GetConfig().songPrice} song tokens.";

        yield return new WaitUntil(() => userConfirmedUnlock != null);

        if (userConfirmedUnlock == true)
        {
            var data = SongManager.Instance.GetGameData();
            int price = SongManager.Instance.GetConfig().songPrice;

            if (data.song_token >= price)
            {
                data.song_token -= price;
                data.unlocked_songs.Add(song.id);
                SetupSongButton(song, songButton); // Recheck unlock status
            }
            else
            {
                Debug.LogWarning("Not enough tokens!");
            }
        }

        unlockConfirmPanel.SetActive(false);
    }

    public void OnConfirmUnlock() => userConfirmedUnlock = true;
    public void OnCancelUnlock() => userConfirmedUnlock = false;

    public void SelectSong(string songID)
    {
        currentSong = SongManager.Instance.FindById(songID);
        if (currentSong == null) return;

        songNameText.text = currentSong.songName;
        songInfoText.text = currentSong.info;

        var scoreData = SongManager.Instance.GetScoreDataBySongID(currentSong.id);
        if (scoreData)
        {
            if (selectedGameMode == GameManager.GameMode.NORMAL)
            {
                songScoreText.text = scoreData.easyScore.ToString();
                songStateText.text = scoreData.easyState;
            }
            else
            {
                songScoreText.text = scoreData.hardScore.ToString();
                songStateText.text = scoreData.hardState;
            }
        }
        else
        {
            if (selectedGameMode == GameManager.GameMode.NORMAL)
            {
                songScoreText.text = "0";
                songStateText.text = "";
            }
            else
            {
                songScoreText.text = "0";
                songStateText.text = "";
            }
        }
    }

    public void EnterGameModeScene()
    {
        StartCoroutine(InitializeGame());
    }

    private IEnumerator InitializeGame()
    {
        AudioManager.Instance.StopBGM();
        loadingPanel.SetActive(true);

        if (currentSong.songClip == null || currentSong.hardMidi == null || currentSong.easyMidi == null)
        {
            bool isDownloadComplete = false;
            SongLoader.Instance.DownloadSong(currentSong, () => isDownloadComplete = true);
            yield return new WaitUntil(() => isDownloadComplete);
        }

        StartGame();
    }

    private void StartGame()
    {
        SongManager.Instance.SetCurrentSelectedGame(currentSong, selectedGameMode);
        GameSceneManager.Instance.LoadScene(gameSceneName);
    }

    private void SetGameMode(GameManager.GameMode mode)
    {
        selectedGameMode = mode;

        if (currentSong == null) return;

        var scoreData = SongManager.Instance.GetScoreDataBySongID(currentSong.id);
        if (scoreData)
        {
            if (selectedGameMode == GameManager.GameMode.NORMAL)
            {
                songScoreText.text = scoreData.easyScore.ToString();
                songStateText.text = scoreData.easyState;
            }
            else
            {
                songScoreText.text = scoreData.hardScore.ToString();
                songStateText.text = scoreData.hardState;
            }
        }
        else
        {
            if (selectedGameMode == GameManager.GameMode.NORMAL)
            {
                songScoreText.text = "0";
                songStateText.text = "";
            }
            else
            {
                songScoreText.text = "0";
                songStateText.text = "";
            }
        }
    }
}
