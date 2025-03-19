using UnityEngine;

public class RestartGameButton : MonoBehaviour
{
    [SerializeField] private VoidPublisherSO restartGameSO;
    [SerializeField] VoidPublisherSO togglePauseGameSO;

    public void RestartGame()
    {
        togglePauseGameSO.RaiseEvent();
        restartGameSO.RaiseEvent();
    }
}
