using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System.Net;

public class SongLoader : MonoBehaviour
{
    private string apiUrl = "https://avnn-server.onrender.com/api/songs";
    public string savePath = "Assets/ScriptableObjects/Songs/"; //Path to save SOs


    void Start()
    {
        StartCoroutine(FetchSongs());
    }

    IEnumerator FetchSongs()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(apiUrl))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string json = request.downloadHandler.text;
                SongData[] songs = JsonHelper.FromJson<SongData>(json);

                foreach (SongData song in songs)
                {
                    CreateSO(song);
                }
            }
            else
            {
                Debug.LogError("Failed to fetch songs: " + request.error);
            }
        }
    }
    public void DownloadSong(SongInfoSO songInfo, System.Action onComplete)
    {
        StartCoroutine(DownloadSongCoroutine(songInfo, onComplete));
    }

    private IEnumerator DownloadSongCoroutine(SongInfoSO songInfo, System.Action onComplete)
    {
        bool audioDone = false, easyMidiDone = false, hardMidiDone = false;

        StartCoroutine(DownloadAudio(songInfo.songClipUrl, (AudioClip clip) => {
            songInfo.songClip = clip;
            audioDone = true;
        }));

        StartCoroutine(DownloadMidi(songInfo.easyMidiUrl, (TextAsset midi) => {
            songInfo.easyMidi = midi;
            easyMidiDone = true;
        }));

        StartCoroutine(DownloadMidi(songInfo.hardMidiUrl, (TextAsset midi) => {
            songInfo.hardMidi = midi;
            hardMidiDone = true;
        }));

        yield return new WaitUntil(() => audioDone && easyMidiDone && hardMidiDone);

        if (songInfo.easyNoteTimings.Count == 0 || songInfo.hardNoteTimings.Count == 0)
        {
            songInfo.GenerateNoteTimings();
        }

        onComplete?.Invoke();
    }
    void CreateSO(SongData song)
    {
        SongInfoSO songInfo = ScriptableObject.CreateInstance<SongInfoSO>();
        songInfo.id = song._id;
        songInfo.songName = song.songName;
        songInfo.composer = song.composer;
        songInfo.genre = song.genre;
        songInfo.BPM = song.bpm;
        songInfo.info = song.info;
        songInfo.songClipUrl = song.audioClip;
        songInfo.easyMidiUrl = song.easyMidi;
        songInfo.hardMidiUrl = song.hardMidi;
           

        // Save the ScriptableObject
        string path = savePath + song.songName.Replace(" ", "_") + ".asset";
        UnityEditor.AssetDatabase.CreateAsset(songInfo, path);
        UnityEditor.AssetDatabase.SaveAssets();

        SongManager.Instance.AddSong(songInfo);

        Debug.Log($"Created SongInfoSO: {song.songName}");
    }

    IEnumerator DownloadMidi(string path, System.Action<TextAsset> callback)
    {
        if (string.IsNullOrEmpty(path)) yield break;
        string name = Path.GetFileName(path);
        string url = apiUrl + "/file/" + name;
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                byte[] midiData = request.downloadHandler.data;

                TextAsset midi = new TextAsset(midiData);
                callback(midi);
                Debug.Log($"Downloaded MIDI!");
            }
            else
            {
                Debug.LogError("Failed to download MIDI: " + request.error);
            }
        }
    }

    IEnumerator DownloadAudio(string filePath, System.Action<AudioClip> callback)
    {
        if (string.IsNullOrEmpty(filePath)) yield break;

        string filename = Path.GetFileName(filePath);
        string url = apiUrl + "/file/" + filename;
        using (UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                AudioClip clip = DownloadHandlerAudioClip.GetContent(request);
                Debug.Log(clip);
                callback(clip);
                Debug.Log("Downloaded Audio!");
            }
            else
            {
                Debug.LogError("Failed to download audio: " + request.error);
            }
        }
    }
}

[System.Serializable]
public class SongData
{
    public string _id;
    public string songName;
    public string composer;
    public string genre;
    public int bpm;
    public string info;
    public string audioClip;
    public string easyMidi;
    public string hardMidi;
    public List<float> easyNoteTimings;
    public List<float> hardNoteTimings;
}

public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        string newJson = "{ \"array\": " + json + "}";
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
        return wrapper.array;
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] array;
    }
}
