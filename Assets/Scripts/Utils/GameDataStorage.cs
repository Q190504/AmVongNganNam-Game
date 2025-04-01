using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections.Generic;

[System.Serializable]
public class ScoreInfo
{
    public string song; // Song ID from MongoDB
    public int easyScore;
    public int hardScore;
}

[System.Serializable]
public class GameStatus
{
    public string userId;
    public string[] unlocked_songs;
    public string[] unlocked_instruments;
    public ScoreInfo[] highscore;
}

public class GameDataStorage : MonoBehaviour
{
    private string apiUrl = "https://avnn-server.onrender.com/api/game-status";
    public string userId;

    public void HandleLoadGameStatus(bool isAuthenticated)
    {
        if (isAuthenticated)
        {
            userId = PlayerPrefs.GetString("UID");
            StartCoroutine(LoadGameStatus());
        }
    }

    public void SaveGameStatus(string[] unlockedSongs, string[] unlockedInstruments, ScoreInfo[] scores)
    {
        GameStatus gameStatus = new GameStatus
        {
            userId = userId,
            unlocked_songs = unlockedSongs,
            unlocked_instruments = unlockedInstruments,
            highscore = scores
        };

        StartCoroutine(SaveGameStatusRequest(gameStatus));
    }

    IEnumerator SaveGameStatusRequest(GameStatus gameStatus)
    {
        string jsonData = JsonUtility.ToJson(gameStatus);
        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Game Status Saved: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Error Saving Game Status: " + request.error);
        }
    }

    IEnumerator LoadGameStatus()
    {
        UnityWebRequest request = UnityWebRequest.Get(apiUrl + "/" + userId);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;
            GameStatus loadedStatus = JsonUtility.FromJson<GameStatus>(json);
            CreateSO(loadedStatus);
            Debug.Log("Loaded Game Status: " + json);
        }
        else
        {
            Debug.LogError("Error loading game status: " + request.error);
            string json = request.downloadHandler.text;
            Debug.Log("Error: " + json);
        }
    }


    void CreateSO(GameStatus status)
    {
        GameDataSO gameData = ScriptableObject.CreateInstance<GameDataSO>();
        gameData.unlocked_songs = status.unlocked_songs;
        gameData.unlocked_instruments = status.unlocked_instruments;
        gameData.highscore = new List<ScoreInfo>();
        foreach(ScoreInfo scoreInfo in status.highscore)
        {
            gameData.highscore.Add(scoreInfo);
        }

        SongManager.Instance.SetGameData(gameData);
    }
}
