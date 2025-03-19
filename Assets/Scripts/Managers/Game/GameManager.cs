using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public enum GameMode
    {
        NORMAL,
        HARD
    }

    [SerializeField] private SongInfoSO songInfo;
    [SerializeField] private List<float> noteTimings;
    [SerializeField] private ConfigSO config;
    [SerializeField] private FloatPublisherSO gameSongUpdatePublisherSO;
    private int nextNoteIndex = 0;

    [SerializeField] private float travelDuration;

    private void CalculateTravelDuration()
    {
        travelDuration = (config.BeatsShownOnScreen / (float)songInfo.BPM) * 60f;
        Debug.Log("Travel Duration: " + travelDuration);
    }

    public void Load(SongInfoSO songInfo, ConfigSO config, GameMode mode)
    {

        this.songInfo = songInfo;
        this.config = config;
        if (mode == GameMode.NORMAL && songInfo.easyNoteTimings.Count!=0)
        {
            this.noteTimings = songInfo.easyNoteTimings;
            Debug.Log("Starting game on easy mode...");
        }
        else if (mode == GameMode.HARD && songInfo.hardNoteTimings.Count != 0)
        {
            this.noteTimings = songInfo.hardNoteTimings;
            Debug.Log("Starting game on hard mode...");
        }
    }    
    private void Start()
    {
        (SongInfoSO, GameMode) selectedGame = SongManager.Instance.GetCurrentSelectedSong();
        Load(selectedGame.Item1, config, selectedGame.Item2);
        CalculateTravelDuration();
        AudioManager.Instance.StopBGM();
        AudioManager.Instance.PlayGameSong(songInfo.songClip);
    }

    private void Update()
    {   
        if (noteTimings.Count == 0) return;
        float songPosInSeconds = AudioManager.Instance.GetGameSongSecond();
        gameSongUpdatePublisherSO.RaiseEvent(songPosInSeconds);
    }

    public void SpawnNote(float songPosInSeconds)
    {
        while (nextNoteIndex < noteTimings.Count &&
               noteTimings[nextNoteIndex] - travelDuration <= songPosInSeconds)
        {
            float noteSpawnTime = noteTimings[nextNoteIndex] - travelDuration;
            GameEvents.TriggerNoteSpawn(noteSpawnTime, travelDuration);
            nextNoteIndex++;
        }
    }
}
