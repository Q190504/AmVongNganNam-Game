using TMPro;
using UnityEngine;

public class TokenShow : MonoBehaviour
{
    public TMP_Text song_token;
    public TMP_Text instrument_token;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        song_token.text = SongManager.Instance.GetGameData().song_token.ToString();
        instrument_token.text = SongManager.Instance.GetGameData().instrument_token.ToString();
    }
}
