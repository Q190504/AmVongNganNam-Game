using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [SerializeField] private SongInfoSO songInfo;
    [SerializeField] private ConfigSO config;
    [SerializeField] private FloatPublisherSO gameSongUpdatePublisherSO;
    private int nextNoteIndex = 0;

    [SerializeField] private float travelDuration;

    private void CalculateTravelDuration()
    {
        travelDuration = (config.BeatsShownOnScreen / songInfo.BPM) * 60f;
        Debug.Log("Travel Duration: " + travelDuration);
    }

    public void Load(SongInfoSO songInfo, ConfigSO config)
    {
        this.songInfo = songInfo;
        this.config = config;
    }    
    private void Start()
    {
        CalculateTravelDuration();
        AudioManager.Instance.PlayGameSong(songInfo.songClip);
    }

    private void Update()
    {
        float songPosInSeconds = AudioManager.Instance.GetGameSongSecond();
        gameSongUpdatePublisherSO.RaiseEvent(songPosInSeconds);
    }

    public void SpawnNote(float songPosInSeconds)
    {
        while (nextNoteIndex < songInfo.noteTimings.Count &&
               songInfo.noteTimings[nextNoteIndex] - travelDuration <= songPosInSeconds)
        {
            float noteSpawnTime = songInfo.noteTimings[nextNoteIndex] - travelDuration;
            GameEvents.TriggerNoteSpawn(noteSpawnTime, travelDuration);
            nextNoteIndex++;
        }
    }
}
