using System.Collections.Generic;
using UnityEngine;

public class SongManager : MonoBehaviour
{
    public static SongManager Instance;
    private List<SongInfoSO> songInfos = new List<SongInfoSO>();
    private GameDataSO gameData;

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

    public void SetGameData(GameDataSO gameData)
    {
        this.gameData = gameData;
    }
}
