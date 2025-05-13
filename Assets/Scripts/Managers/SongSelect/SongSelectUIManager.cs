using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;

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

    [Header("Sprites")]
    [SerializeField] private Sprite selectedButtonSprite;
    [SerializeField] private Sprite nonselectedButtonSprite;    
    [SerializeField] private Sprite selectedSongButtonSprite;
    [SerializeField] private Sprite nonselectedSongButtonSprite;

    [Header("Event SOs")]
    [SerializeField] private VoidPublisherSO unlockEvent;


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
        SetDifficultyButtonsSprite(selectedGameMode);

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
        bool hasNewSong = false;
        for (int i = 0; i < songList.Count; i++)
        {
            int index = i; // capture index for lambda
            var song = songList[index];
            var songButton = Instantiate(songButtonPrefab, songListContentPanel);

            songButton.GetComponentInChildren<TextMeshProUGUI>().text = song.songName;
            songButtonList.Add(songButton);

            hasNewSong = SetupSongButton(song, songButton);

            if (hasNewSong)
            {
                SaveSongList();
            }
        }
    }

    private void SaveSongList()
    {
        GameDataSO gameData = SongManager.Instance.GetGameData();
        string[] new_unlocked_song = gameData.unlocked_songs.ToArray();
        int song_token = gameData.song_token;
        GameDataStorage.Instance.SaveGameStatus(new_unlocked_song, null, null, song_token, -1);
    }
    private bool SetupSongButton(SongInfoSO song, GameObject songButton)
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
            return true;
        }
        else
        {
            songButton.GetComponent<Button>().onClick.AddListener(() => UnlockSong(song, songButton));
        }
        return false;
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
                SaveSongList();
                SetupSongButton(song, songButton); // Recheck unlock status
                unlockEvent.RaiseEvent();
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

        foreach (GameObject songButton in songButtonList)
        {
            TMP_Text buttonText = songButton.GetComponentInChildren<TMP_Text>();
            if (buttonText.text == currentSong.songName)
                songButton.GetComponent<Image>().sprite = selectedSongButtonSprite;
            else
                songButton.GetComponent<Image>().sprite = nonselectedSongButtonSprite;
        }

        songNameText.text = currentSong.songName;
        songInfoText.text = currentSong.info;

        var scoreData = SongManager.Instance.GetScoreDataBySongID(currentSong.id);
        if (scoreData)
        {
            if (selectedGameMode == GameManager.GameMode.NORMAL)
            {
                songScoreText.text = scoreData.easyScore.ToString();
                songStateText.text = scoreData.easyState.ToString();
            }
            else
            {
                songScoreText.text = scoreData.hardScore.ToString();
                songStateText.text = scoreData.hardState.ToString();
            }
        }
        else
        {
            if (selectedGameMode == GameManager.GameMode.NORMAL)
            {
                songScoreText.text = "0";
                songStateText.text = ConfigSO.CompletionState.NOT_COMPLETED.ToString();
            }
            else
            {
                songScoreText.text = "0";
                songStateText.text = ConfigSO.CompletionState.NOT_COMPLETED.ToString();
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
        SetDifficultyButtonsSprite(selectedGameMode);
        if (currentSong == null) return;

        var scoreData = SongManager.Instance.GetScoreDataBySongID(currentSong.id);
        if (scoreData)
        {
            if (selectedGameMode == GameManager.GameMode.NORMAL)
            {
                songScoreText.text = scoreData.easyScore.ToString();
                songStateText.text = scoreData.easyState.ToString();
            }
            else
            {
                songScoreText.text = scoreData.hardScore.ToString();
                songStateText.text = scoreData.hardState.ToString();
            }
        }
        else
        {
            if (selectedGameMode == GameManager.GameMode.NORMAL)
            {
                songScoreText.text = "0";
                songStateText.text = ConfigSO.CompletionState.NOT_COMPLETED.ToString();
            }
            else
            {
                songScoreText.text = "0";
                songStateText.text = ConfigSO.CompletionState.NOT_COMPLETED.ToString();
            }
        }
    }

    public void SetDifficultyButtonsSprite(GameMode mode)
    { 
        switch(mode)
        {
            case GameMode.NORMAL:
                easyModeButton.GetComponent<Image>().sprite = selectedButtonSprite;
                hardModeButton.GetComponent<Image>().sprite = nonselectedButtonSprite;
                break;
            case GameMode.HARD:
                hardModeButton.GetComponent<Image>().sprite = selectedButtonSprite;
                easyModeButton.GetComponent<Image>().sprite = nonselectedButtonSprite;
                break;
        }
    }
}
