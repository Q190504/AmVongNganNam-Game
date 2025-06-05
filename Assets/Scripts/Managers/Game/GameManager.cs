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
    [SerializeField] private BoolPublisherSO endGamePublisher;
    private int nextNoteIndex = 0;
    private float songLength = 0;

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

        Debug.Log($"Selected song: {selectedGame.Item1.songName}");
        Debug.Log($"Selected songClip: {(selectedGame.Item1.songClip == null ? "NULL" : selectedGame.Item1.songClip.name)}");

        Load(selectedGame.Item1, config, selectedGame.Item2);

        CalculateTravelDuration();

        if (selectedGame.Item1.songClip == null)
        {
            Debug.LogError("songClip is NULL! Audio cannot be played.");
        }
        else
        {
            Debug.Log($"Playing song clip: {selectedGame.Item1.songClip.name}");
            Debug.Log($"Clip length (before play): {selectedGame.Item1.songClip.length} seconds");
        }

        AudioManager.Instance.PlayGameSong(selectedGame.Item1.songClip);

        this.songLength = selectedGame.Item1.songLength;
        Debug.Log($"Song length stored in songInfo: {this.songLength} seconds");
    }

    private float songPosTracker = 0f;
    private void Update()
    {
        if (noteTimings.Count == 0) return;

        float songPosInSeconds = AudioManager.Instance.GetGameSongSecond();
        if (songPosTracker <= songPosInSeconds)
            songPosTracker = songPosInSeconds;
        else if (songPosInSeconds == 0 && nextNoteIndex >= noteTimings.Count)
            songPosTracker += Time.deltaTime;

        if (songPosInSeconds >= songLength 
            || (songPosInSeconds == 0 && songPosTracker >= songLength && nextNoteIndex >= noteTimings.Count))
        {
            songPosTracker = 0f;
            Debug.Log("Ending game");
            endGamePublisher.RaiseEvent(true);
        }

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

    public void RestartGame()
    {
        songPosTracker = 0f;
        nextNoteIndex = 0;
    }
}
