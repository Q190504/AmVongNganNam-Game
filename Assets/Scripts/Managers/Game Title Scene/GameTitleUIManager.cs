using TMPro;
using UnityEngine;

public class GameTitleUIManager : MonoBehaviour
{
    [Header("Login Panel")]
    public GameObject loginPanel;
    public TMP_Text errorText;

    [Header("Play Panel")]
    public GameObject playPanel;

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

    public void SetErrorText(string error)
    {
        errorText.gameObject.SetActive(true);
        errorText.text = error; 
    }

    public void UpdateUI(bool isLoggedIn)
    {
        if (isLoggedIn)
        {
            errorText.gameObject.SetActive(false);
            SetLoginPanel(false);
            SetPlayPanel(true);
        }
        else
        {
            SetLoginPanel(true);
            SetPlayPanel(false);
        }
    }
}
