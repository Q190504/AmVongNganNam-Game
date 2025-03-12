using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSong", menuName = "RhythmGame/SongInfo")]
public class SongInfoSO : ScriptableObject
{
    public string id;
    public string songName;
    public string composer;
    public string genre;
    public int BPM;
    public string info;
    public string songClipUrl;
    public string easyMidiUrl;
    public string hardMidiUrl;
    public AudioClip songClip;
    public TextAsset easyMidi;
    public TextAsset hardMidi;
    public List<float> easyNoteTimings;
    public List<float> hardNoteTimings;
    public void GenerateNoteTimings()
    {
        if (easyMidi != null)
        {
            easyNoteTimings = MidiParser.ParseMidi(easyMidi, BPM);
            Debug.Log($"Generated {easyNoteTimings.Count} notes for {songName}");
        }
        else
        {
            Debug.LogError("No easy MIDI file assigned!");
        }
        if (hardMidi != null)
        {
            hardNoteTimings = MidiParser.ParseMidi(hardMidi, BPM);
            Debug.Log($"Generated {hardNoteTimings.Count} notes for {songName}");
        }
        else
        {
            Debug.LogError("No hard MIDI file assigned!");
        }
    }
}
