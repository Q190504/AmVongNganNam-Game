using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

using System;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class ScoreInfo
{
    public string _id;          // MongoDB ScoreInfo ID
    public string user_id;      // MongoDB Account ID
    public string song_id;         // MongoDB Song ID
    public int easyScore;
    public string easyState;
    public int hardScore;
    public string hardState;
}

[System.Serializable]
public class GameStatus
{
    public string _id;                  // MongoDB GameStatus ID
    public string user_id;
    public string[] unlocked_songs;
    public string[] unlocked_instruments;
    public ScoreInfo[] highscore;      // Matches the array from the JSON
    public int song_token;
    public int instrument_token;
}

public class GameDataStorage : MonoBehaviour
{
    private string apiUrl = ENV_CONFIG.STAT_URL;
    public bool IsDoneLoading { get; private set; }
    public static GameDataStorage Instance { get; private set; }
    public StringPublisherSO errorPublisher;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            this.IsDoneLoading = false;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void HandleLoadGameStatus(bool isAuthenticated)
    {
        if (isAuthenticated)
        {
            StartCoroutine(LoadGameStatus(() => IsDoneLoading = true));
        }
    }

    public void SaveGameStatus(string[] unlockedSongs = null, string[] unlockedInstruments = null, ScoreInfo[] scores = null, int song_token = -1, int instrument_token = -1)
    {
        StartCoroutine(SaveGameStatusRequest(unlockedSongs, unlockedInstruments, scores, song_token, instrument_token));
    }

    IEnumerator SaveGameStatusRequest(string[] unlockedSongs, string[] unlockedInstruments, ScoreInfo[] scores, int song_token = -1, int instrument_token = -1)
    {
        GameStatus stat = new GameStatus();
        
        bool changed = false;

        if (unlockedSongs != null && unlockedSongs.Length > 0)
        {
            stat.unlocked_songs = unlockedSongs;
            changed = true;
        }    
            

        if (unlockedInstruments != null && unlockedInstruments.Length > 0)
        {
            stat.unlocked_instruments = unlockedInstruments;
            changed = true;
        }    
            

        if (scores != null && scores.Length > 0)
        {
            stat.highscore = scores;
            changed = true;
        }     

        if (instrument_token >= 0 || song_token >= 0)
        {
            changed = true;
        }

        stat.instrument_token = instrument_token;
        stat.song_token = song_token;

        if (!changed)
        {
            Debug.LogWarning("No data to update.");
            yield break;
        }

       

        string jsonData = JsonUtility.ToJson(stat);
        Debug.Log(jsonData);
        UnityWebRequest request = new UnityWebRequest(apiUrl, "PUT");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        string token = PlayerPrefs.GetString("token", "");
        if (!string.IsNullOrEmpty(token))
        {
            request.SetRequestHeader("Authorization", "Bearer " + token);
        }
        else
        {
            Debug.Log("No token found in PlayerPrefs.");
        }
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Game Status Saved: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Error Saving Game Status: " + request.error);
            errorPublisher.RaiseEvent("Failed to save game status: " + request.error);
        }
    }


    IEnumerator LoadGameStatus(System.Action onComplete)
    {
        string url = apiUrl;

        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SetRequestHeader("Content-Type", "application/json");
        string token = PlayerPrefs.GetString("token", "");
        if (!string.IsNullOrEmpty(token))
        {
            request.SetRequestHeader("Authorization", "Bearer " + token);
        }
        else
        {
            Debug.Log("No token found in PlayerPrefs.");
        }
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;
            Debug.Log("Loaded Game Status: " + json);
            try
            {
                GameStatus loadedStatus = JsonUtility.FromJson<GameStatus>(json);
                CreateSO(loadedStatus);
                onComplete?.Invoke();

            }
            catch (Exception ex)
            {
                Debug.LogError("JSON parsing failed: " + ex.Message);
                errorPublisher.RaiseEvent("JSON parsing failed: " + ex.Message);
            }
        }
        else
        {
            Debug.LogError("Failed to load game status: " + request.error);
            errorPublisher.RaiseEvent("Failed to load game status: " + request.error);
        }
        
    }


    void CreateSO(GameStatus status)
    {
        GameDataSO gameData = ScriptableObject.CreateInstance<GameDataSO>();
        gameData.unlocked_songs = status.unlocked_songs.ToList();
        gameData.unlocked_instruments = status.unlocked_instruments.ToList();
        gameData.highscore = new List<ScoreDataSO>();
        gameData.instrument_token = status.instrument_token;
        gameData.song_token = status.song_token;
        foreach (ScoreInfo scoreId in status.highscore)
        {
            ScoreDataSO temp = ScriptableObject.CreateInstance<ScoreDataSO>();
            temp.song_id = scoreId.song_id;
            temp.easyScore = scoreId.easyScore;
            temp.easyState = ConfigSO.mapStringToState(scoreId.easyState);
            temp.hardScore = scoreId.hardScore;
            temp.hardState = ConfigSO.mapStringToState(scoreId.hardState);
            gameData.highscore.Add(temp);
        }

        SongManager.Instance.SetGameData(gameData);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T payload;
    }

}
