using System;
using System.Collections.Generic;
using UnityEngine;

public class SongManager : MonoBehaviour
{
    public static SongManager Instance;
    private List<SongInfoSO> songInfos = new List<SongInfoSO>();
    private GameDataSO gameData;
    [SerializeField]
    private ConfigSO config;

    private (SongInfoSO, GameManager.GameMode) currentSelectedGame;

    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void SaveSongList()
    {
        GameDataSO gameData = SongManager.Instance.GetGameData();
        string[] new_unlocked_song = gameData.unlocked_songs.ToArray();
        int song_token = gameData.song_token;
        GameDataStorage.Instance.SaveGameStatus(new_unlocked_song, null, null, song_token, -1);
    }
    public void SetCurrentSelectedGame(SongInfoSO songInfo, GameManager.GameMode mode)
    {
        this.currentSelectedGame = (songInfo, mode);
    }

    public (SongInfoSO, GameManager.GameMode) GetCurrentSelectedSong()
    {
        return currentSelectedGame;
    }
    public void AddSong(SongInfoSO songInfo)
    {
        songInfos.Add(songInfo);
    }

    public SongInfoSO FindById(string id)
    {
        return songInfos.Find(song => song.id == id);
    }

    public List<SongInfoSO> GetSongInfos()
    {
        return songInfos;
    }

    public GameDataSO GetGameData()
    {
        return gameData;
    }
    public ConfigSO GetConfig()
    {
        return config;
    }
    public ScoreDataSO GetScoreDataBySongID(string id)
    {
        return gameData.highscore.Find(score => score.song_id == id);
    }

    public bool IsSongIdInData(string id)
    {
        return gameData.unlocked_songs.Contains(id);
    }

    public bool IsInstIdInData(string id)
    {
        return gameData.unlocked_instruments.Contains(id);
    }

    public void SetGameData(GameDataSO gameData)
    {
        this.gameData = gameData;
    }
}
