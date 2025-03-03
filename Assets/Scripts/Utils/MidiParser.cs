using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.IO;

public static class MidiParser
{
    public static List<float> ParseMidi(TextAsset midiFile, float BPM)
    {
        List<float> noteTimings = new List<float>();

        if (midiFile == null)
        {
            Debug.LogError("MIDI file is null!");
            return noteTimings;
        }

        try
        {
            MemoryStream stream = new MemoryStream(midiFile.bytes);
            MidiFile midi = MidiFile.Read(stream);

            TempoMap tempoMap = midi.GetTempoMap();

            foreach (var note in midi.GetNotes())
            {
                MetricTimeSpan metricTime = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, tempoMap);
                float timeInSeconds = (float)metricTime.TotalSeconds;
                noteTimings.Add(timeInSeconds);
            }

            noteTimings.Sort();
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error parsing MIDI file: " + ex.Message);
        }

        return noteTimings;
    }
}
