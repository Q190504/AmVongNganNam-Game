using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class LibraryUIManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject songPanel;
    public GameObject instrumentPanel;

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
        instrumentList = InstrumentManager.Instance.instrumentList;
        songButtonList = new List<GameObject>();
        instrumentButtonList = new List<GameObject>();

        for (int i = 0; i < songList.Count; i++)
        {
            int index = i; // captures the current value of i to prevent all lambdas from referencing the same final value of i.

            GameObject SongButton = Instantiate(songButtonPrefab, songListContentPanel);

            SongButton.GetComponentInChildren<TextMeshProUGUI>().text = GetSongName(index);
            SongButton.GetComponent<Button>().onClick.AddListener(() => SelectSong(index));

            songButtonList.Add(SongButton);
        }

        for (int i = 0; i < instrumentList.Count; i++)
        {
            int index = i; // captures the current value of i to prevent all lambdas from referencing the same final value of i.

            GameObject instrumentButton = Instantiate(instrumentButtonPrefab, instrumentListContentPanel);

            instrumentButton.GetComponentInChildren<TextMeshProUGUI>().text = GetInstrumentName(index);
            instrumentButton.GetComponent<Button>().onClick.AddListener(() => SelectInstrument(index));

            instrumentButtonList.Add(instrumentButton);
        }

        SelectSong(0);
        SelectInstrument(0);

        songPanel.SetActive(true);
        instrumentPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

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

    public string GetInstrumentName(int index)
    {
        if (index >= 0 && index < instrumentList.Count)
            return instrumentList[index].instrumentName;
        else return "null";
    }

    public void SelectInstrument(int index)
    {
        if (index >= 0 && index < instrumentList.Count)
        {
            currentInstrument = instrumentList[index];
            instrumentNameText.text = currentInstrument.instrumentName;
            instrumentInfoText.text = currentInstrument.instrumentInfo;
            instrumentImage.sprite = currentInstrument.instrumentImage;
        }
    }

    public void SwitchToSongPanel()
    {
        songPanel.SetActive(true);
        instrumentPanel.SetActive(false);
    }

    public void SwitchToInstrumentPanel()
    {
        instrumentPanel.SetActive(true);
        songPanel.SetActive(false);
    }
}
