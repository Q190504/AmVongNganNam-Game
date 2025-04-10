using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.PointerEventData;


public class LibraryUIManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject songPanel;
    public GameObject instrumentPanel;

    [Header("Buttons")]
    [SerializeField] private Button switchToInstrumentPanelButton;
    [SerializeField] private Button switchToSongPanelButton;

    [Header("Sprites")]
    [SerializeField] private Sprite selectedButtonSprite;
    [SerializeField] private Sprite nonselectedButtonSprite;

    [Header("Song")]
    public List<SongInfoSO> songList;
    public GameObject songButtonPrefab;
    private List<GameObject> songButtonList;
    public Transform songListContentPanel;
    public TMP_Text songNameText;
    public TMP_Text songInfoText;
    private SongInfoSO currentSong;

    [Header("Instrument")]
    public List<InstrumentDataSO> instrumentList;
    public GameObject instrumentButtonPrefab;
    private List<GameObject> instrumentButtonList;
    public Transform instrumentListContentPanel;
    public TMP_Text instrumentNameText;
    public TMP_Text instrumentInfoText;
    public Image instrumentImage;
    private InstrumentDataSO currentInstrument;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        songList = SongManager.Instance.GetSongInfos();
        instrumentList = InstrumentManager.Instance.GetInstrumentDatas();
        songButtonList = new List<GameObject>();
        instrumentButtonList = new List<GameObject>();

        GenerateSongButtons();
        GenerateInstrumentButtons();

        SelectSong(songList[0].id);
        SelectInstrument(instrumentList[0].instrumentId);

        SwitchToSongPanel();

        instrumentPanel.SetActive(false);
    }

    private void GenerateInstrumentButtons()
    {
        bool hasNewInst = false;
        for (int i = 0; i < instrumentList.Count; i++)
        {
            int index = i; // capture index for lambda
            var inst = instrumentList[index];
            var instButton = Instantiate(instrumentButtonPrefab, instrumentListContentPanel);

            instButton.GetComponentInChildren<TextMeshProUGUI>().text = inst.instrumentName;
            instButton.GetComponent<Image>().sprite = nonselectedButtonSprite;
            instrumentButtonList.Add(instButton);

            hasNewInst = SetupInstrumentButton(inst, instButton);

            if (hasNewInst)
            {
                SaveInstrumentList();
            }
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
            songButton.GetComponent<Image>().sprite = nonselectedButtonSprite;
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
        GameDataStorage.Instance.SaveGameStatus(new_unlocked_song, null, null);
    }

    private void SaveInstrumentList()
    {
        GameDataSO gameData = SongManager.Instance.GetGameData();
        string[] new_unlocked_instrument = gameData.unlocked_instruments.ToArray();
        GameDataStorage.Instance.SaveGameStatus(null, new_unlocked_instrument, null);
    }
    private bool SetupInstrumentButton(InstrumentDataSO inst, GameObject instButton)
    {
        var data = SongManager.Instance.GetGameData();

        if (SongManager.Instance.IsInstIdInData(inst.instrumentId))
        {
            UnlockInstrumentUI(inst, instButton);
        }
        else if (inst.isDefault)
        {
            data.unlocked_instruments.Add(inst.instrumentId);
            UnlockInstrumentUI(inst, instButton);
            return true;
        }
        return false;
    }

    private void UnlockInstrumentUI(InstrumentDataSO inst, GameObject instButton)
    {
        instButton.transform.Find("LockPanel").gameObject.SetActive(false);
        instButton.GetComponent<Button>().onClick.RemoveAllListeners();
        instButton.GetComponent<Button>().onClick.AddListener(() => SelectInstrument(inst.instrumentId));
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
        return false;
    }

    private void UnlockSongUI(SongInfoSO song, GameObject songButton)
    {
        songButton.transform.Find("LockPanel").gameObject.SetActive(false);
        songButton.GetComponent<Button>().onClick.RemoveAllListeners();
        songButton.GetComponent<Button>().onClick.AddListener(() => SelectSong(song.id));
    }

    public void SelectSong(string songID)
    {
        currentSong = SongManager.Instance.FindById(songID);
        if (currentSong == null) return;

        songNameText.text = currentSong.songName;
        songInfoText.text = currentSong.info;

        foreach (GameObject songButton in songButtonList)
        {
            TMP_Text buttonText = songButton.GetComponentInChildren<TMP_Text>();
            if (buttonText.text == currentSong.songName)
                songButton.GetComponent<Image>().sprite = selectedButtonSprite;
            else
                songButton.GetComponent<Image>().sprite = nonselectedButtonSprite;
        }
    }

    public void SelectInstrument(string id)
    {
        currentInstrument = InstrumentManager.Instance.FindById(id);
        instrumentNameText.text = currentInstrument.instrumentName;
        instrumentInfoText.text = currentInstrument.instrumentInfo;
        instrumentImage.sprite = currentInstrument.instrumentImage;

        foreach (GameObject instrumentButton in instrumentButtonList)
        {
            TMP_Text buttonText = instrumentButton.GetComponentInChildren<TMP_Text>();
            if (buttonText.text == currentInstrument.instrumentName)
                instrumentButton.GetComponent<Image>().sprite = selectedButtonSprite;
            else
                instrumentButton.GetComponent<Image>().sprite = nonselectedButtonSprite;
        }
    }

    public void SwitchToSongPanel()
    {
        songPanel.SetActive(true);
        switchToSongPanelButton.GetComponent<Image>().color = new Color(100f / 255f, 251f / 255f, 250f / 255f, 255f);
        switchToInstrumentPanelButton.GetComponent<Image>().color = Color.white;
        instrumentPanel.SetActive(false);
    }

    public void SwitchToInstrumentPanel()
    {
        instrumentPanel.SetActive(true);
        switchToInstrumentPanelButton.GetComponent<Image>().color = new Color(100f / 255f, 251f / 255f, 250f / 255f, 255f);
        switchToSongPanelButton.GetComponent<Image>().color = Color.white;
        songPanel.SetActive(false);
    }
}
