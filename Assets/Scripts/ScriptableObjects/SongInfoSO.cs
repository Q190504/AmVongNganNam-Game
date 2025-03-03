using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSong", menuName = "RhythmGame/SongInfo")]
public class SongInfoSO : ScriptableObject
{
    public string songName;
    public string artist;
    public float BPM;
    public AudioClip songClip;
    public TextAsset midiFile;
    public List<float> noteTimings;

    public void GenerateNoteTimings()
    {
        if (midiFile != null)
        {
            noteTimings = MidiParser.ParseMidi(midiFile, BPM);
            Debug.Log($"Generated {noteTimings.Count} notes for {songName}");
        }
        else
        {
            Debug.LogError("No MIDI file assigned!");
        }
    }
}
