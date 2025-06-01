using System.Collections;
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
        StartCoroutine(InitializeGame());
    }

    private IEnumerator InitializeGame()
    {

        // Chờ tải xong hoặc hết thời gian timeout
        yield return new WaitUntil(() =>
        {
            return (SongLoader.Instance?.IsDoneLoading ?? false) &&
                   (GameDataStorage.Instance?.IsDoneLoading ?? false);
        });

        startButton.SwitchScene();
    }
}
