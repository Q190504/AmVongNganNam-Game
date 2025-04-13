using UnityEngine;

public class PauseButton : BaseButton
{
    [SerializeField] private VoidPublisherSO togglePauseGameSO;

    public void PauseGame()
    {
        ButtonClick();
        PauseManager.Instance.TogglePause();
        togglePauseGameSO.RaiseEvent();
    }
}
