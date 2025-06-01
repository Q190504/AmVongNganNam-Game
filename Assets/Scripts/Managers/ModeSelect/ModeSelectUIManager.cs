using TMPro;
using UnityEngine;

public class ModeSelectUIManager : MonoBehaviour
{
    public TMP_Text username;
    
    void Start()
    {
        if (!AudioManager.Instance.IsBGMPlaying())
        {
            AudioManager.Instance.PlayBGM();
        }
        username.text = PlayerPrefs.GetString("name");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
