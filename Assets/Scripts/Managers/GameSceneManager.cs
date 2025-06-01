using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameSceneManager : MonoBehaviour
{
    private static GameSceneManager _instance;
    [SerializeField] private const int NODE_SELECTION_SCENE = 1;

    public static GameSceneManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindFirstObjectByType<GameSceneManager>();
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
            Destroy(this.gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScene(string sceneName)
    {
        string[] parts = sceneName.Split('/');
        string actualSceneName = parts[0];
        float delay = 0f;

        if (parts.Length > 1 && float.TryParse(parts[1], out float parsedDelay))
        {
            delay = parsedDelay;
        }

        StartCoroutine(LoadAsynchronously(actualSceneName, delay));
    }


    IEnumerator LoadAsynchronously(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        //loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            //slider.value = progress;
            //progressText.text = progress * 100 + "%";

            yield return null;
        }
    }

    [Header("Error Panel")]
    public GameObject errorPanel;
    public TMP_Text errorMessageText;
    public void ShowError(string message)
    {
        if (errorPanel == null)
        {
            errorPanel = GameObject.Find("Error Panel");
            if (errorPanel == null)
            {
                Debug.LogWarning("ErrorPanel not found in scene!");
                return;
            } else
            {
                CanvasGroup canvasGroup = errorPanel.GetComponent<CanvasGroup>();
                canvasGroup.alpha = 1f;           // fully visible
                canvasGroup.interactable = true;  // allow interaction
                canvasGroup.blocksRaycasts = true;// block clicks behind the panel

            }
        }

        if (errorMessageText == null)
        {
            errorMessageText = errorPanel.GetComponentInChildren<TMP_Text>();
            if (errorMessageText == null)
            {
                Debug.LogWarning("TMP_Text component not found inside ErrorPanel!");
                return;
            }
        }

        errorPanel.SetActive(true);
        errorMessageText.text = message;
    }

}
