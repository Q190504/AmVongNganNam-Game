using System.Collections.Generic;
using UnityEngine;

public class SongManager : MonoBehaviour
{
    public static SongManager Instance;
    public List<SongInfoSO> songInfos = new List<SongInfoSO>();

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

    public void AddSong(SongInfoSO songInfo)
    {
        songInfos.Add(songInfo);
    }

    public SongInfoSO FindById(string id)
    {
        return songInfos.Find(song => song.id == id);
    }
}
