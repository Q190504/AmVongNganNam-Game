using System;
using System.Collections.Generic;
using UnityEngine;

public static class GameEvents
{
    public static event Action<float, float> OnNoteSpawn;
    public static event Action<float> OnSongTimeUpdated;
    public static event Action<GameObject> OnNoteEvent;
    public static event Action<AudioClip> OnAudioEvent;
    public static void TriggerNoteSpawn(float spawnTime, float travelDuration)
    {
        OnNoteSpawn?.Invoke(spawnTime, travelDuration);
    }

    public static void TriggerSongTimeUpdated(float songTime)
    {
        OnSongTimeUpdated?.Invoke(songTime);
    }

    public static void TriggerNoteEvent(GameObject note)
    {
        OnNoteEvent?.Invoke(note);
    }

    public static void TriggerAudioEvent(AudioClip clip)
    {
        OnAudioEvent?.Invoke(clip);
    }
}
