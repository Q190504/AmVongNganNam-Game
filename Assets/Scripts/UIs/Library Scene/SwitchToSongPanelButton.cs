using UnityEngine;

public class SwitchToSongPanelButton : MonoBehaviour
{
    [SerializeField] private VoidPublisherSO switchToSongPanelSO;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchToSongPanel()
    {
        switchToSongPanelSO.RaiseEvent();
    }
}
