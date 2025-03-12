using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SongSelectUIManager : MonoBehaviour
{
    [Header("Song")]
    public List<SongInfoSO> songList;
    public GameObject songButtonPrefab;
    private List<GameObject> songButtonList;
    public Transform songListContentPanel;
    public TMP_Text songNameText;
    public TMP_Text songInfoText;

    [Header("UI")]
    public Button gameStartButton;
    public Button easyModeButton;
    public Button hardModeButton;
    public GameObject loadingPanel;
    public string gameSceneName;

    private SongInfoSO currentSong;
    private GameManager.GameMode selectedGameMode;

    void Start()
    {
        loadingPanel.SetActive(false);
        songList = SongManager.Instance.songInfos;
        selectedGameMode = GameManager.GameMode.NORMAL;
        songButtonList = new List<GameObject>();

        for (int i = 0; i < songList.Count; i++)
        {
            int index = i; // captures the current value of i to prevent all lambdas from referencing the same final value of i.

            GameObject SongButton = Instantiate(songButtonPrefab, songListContentPanel);

            SongButton.GetComponentInChildren<TextMeshProUGUI>().text = GetSongName(index);
            SongButton.GetComponent<Button>().onClick.AddListener(() => SelectSong(index));

            songButtonList.Add(SongButton);
        }
        easyModeButton.onClick.AddListener(() => SetGameMode(GameManager.GameMode.NORMAL));
        hardModeButton.onClick.AddListener(() => SetGameMode(GameManager.GameMode.HARD));
        gameStartButton.onClick.AddListener(() => StartCoroutine(InitializeGame()));
        SelectSong(0);
    }

    public string GetSongName(int index)
    {
        if (index >= 0 && index < songList.Count)
            return songList[index].songName;
        else return "null";
    }

    public void SelectSong(int index)
    {
        if (index >= 0 && index < songList.Count)
        {
            currentSong = songList[index];
            songNameText.text = currentSong.songName;
            songInfoText.text = currentSong.info;
        }
    }

    private IEnumerator InitializeGame()
    {
        loadingPanel.SetActive(true);

        if (currentSong.songClip == null || currentSong.hardMidi == null || currentSong.easyMidi == null)
        {
            bool isDownloadComplete = false;
            SongLoader.Instance.DownloadSong(currentSong, () => isDownloadComplete = true);
            yield return new WaitUntil(() => isDownloadComplete);
        }
        
        StartGame(); // Start game after download
    }

    void StartGame()
    {
        SongManager.Instance.SetCurrentSelectedGame(currentSong, selectedGameMode);
        GameSceneManager.Instance.LoadScene(gameSceneName);
    }

    void SetGameMode(GameManager.GameMode mode)
    {
        this.selectedGameMode = mode;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
