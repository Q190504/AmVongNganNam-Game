using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SongInfoSO))]
public class SongInfoEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SongInfoSO songInfo = (SongInfoSO)target;

        if (GUILayout.Button("Generate Note Timings from MIDI"))
        {
            songInfo.GenerateNoteTimings();
            EditorUtility.SetDirty(songInfo);
        }
    }
}
