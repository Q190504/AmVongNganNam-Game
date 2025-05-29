using TMPro;
using UnityEngine;

public class ModeSelectUIManager : MonoBehaviour
{
    public TMP_Text username;
    
    void Start()
    {
        username.text = PlayerPrefs.GetString("name");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
