using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameTitleUIManager : MonoBehaviour
{
    [Header("Login Panel")]
    public GameObject loginPanel;
    public TMP_Text errorText;
    public TMP_Text playerText;

    [Header("Play Panel")]
    public GameObject playPanel;
    public SwitchSceneButton startButton;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        errorText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetLoginPanel(bool status)
    {
        loginPanel.SetActive(status);
    }

    public void SetPlayPanel(bool status)
    {
        playPanel.SetActive(status);
    }

    public void UpdateUI(bool isLoggedIn)
    {
        if (isLoggedIn)
        {
            errorText.gameObject.SetActive(false);
            playerText.text = "Xin chào! " + PlayerPrefs.GetString("name");
            SetLoginPanel(false);
            SetPlayPanel(true);
        }
        else
        {
            SetLoginPanel(true);
            SetPlayPanel(false);
        }
    }

    public void EnterGame()
    {
        if (PlayerPrefs.HasKey("token"))
        {
            string token = PlayerPrefs.GetString("token");
            if (!string.IsNullOrEmpty(token))
            {
                StartCoroutine(InitializeGame());
            }
            else
            {
                Debug.LogWarning("Token is empty.");
                UpdateUI(false);
            }
        }
        else
        {
            Debug.LogWarning("Token not found in PlayerPrefs.");
            UpdateUI(false);
        }
    }

    private IEnumerator InitializeGame()
    {

        // Chờ tải xong hoặc hết thời gian timeout
        yield return new WaitUntil(() =>
        {
            return (SongLoader.Instance?.IsDoneLoading ?? false) &&
                   (GameDataStorage.Instance?.IsDoneLoading ?? false);
        });

        var data = SongManager.Instance.GetGameData();

        var instrumentsCopy = new List<string>(data.unlocked_instruments); // make a copy

        bool instChanged = false;
        foreach (string id in instrumentsCopy)
        {
            if (!InstrumentManager.Instance.FindById(id))
            {
                data.unlocked_instruments.Remove(id);
                instChanged = true;
            }
        }

        if (instChanged)
        {
            InstrumentManager.Instance.SaveInstrumentList();
        }

        bool songChanged = false;
        var songsCopy = new List<string>(data.unlocked_songs); // make a copy
        foreach (string id in songsCopy)
        {
            if (!SongManager.Instance.FindById(id))
            {
                data.unlocked_songs.Remove(id);
                songChanged = true;
            }
        }

        if (songChanged)
            SongManager.Instance.SaveSongList();
        if (instChanged)
            InstrumentManager.Instance.SaveInstrumentList();

        startButton.SwitchScene();
    }
}
