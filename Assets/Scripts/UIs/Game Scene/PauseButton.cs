using UnityEngine;

public class PauseButton : MonoBehaviour
{
    [SerializeField] private VoidPublisherSO togglePauseGameSO;

    public void PauseGame()
    {
        togglePauseGameSO.RaiseEvent();
    }
}
