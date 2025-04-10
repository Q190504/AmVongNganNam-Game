using TMPro;
using UnityEngine;

public class ModeSelectUIManager : MonoBehaviour
{
    public TMP_Text username;
    public TMP_Text song_token;
    public TMP_Text instrument_token;
    void Start()
    {
        username.text = PlayerPrefs.GetString("name");
        song_token.text = SongManager.Instance.GetGameData().song_token.ToString();
        instrument_token.text = SongManager.Instance.GetGameData().instrument_token.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
