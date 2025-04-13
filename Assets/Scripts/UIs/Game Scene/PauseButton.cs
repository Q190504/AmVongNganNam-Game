using UnityEngine;

public class PauseButton : BaseButton
{
    [SerializeField] private VoidPublisherSO togglePauseGameSO;

    public void PauseGame()
    {
        ButtonClick();
        togglePauseGameSO.RaiseEvent();
    }
}
