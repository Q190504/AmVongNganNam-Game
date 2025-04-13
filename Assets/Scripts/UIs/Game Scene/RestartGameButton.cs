using UnityEngine;

public class RestartGameButton : BaseButton
{
    [SerializeField] private VoidPublisherSO restartGameSO;
    [SerializeField] VoidPublisherSO togglePauseGameSO;

    public void RestartGame()
    {
        ButtonClick();
        togglePauseGameSO.RaiseEvent();
        restartGameSO.RaiseEvent();
    }
}
