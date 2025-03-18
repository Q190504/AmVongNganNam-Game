using UnityEngine;

public class RestartGameButton : MonoBehaviour
{
    [SerializeField] private VoidPublisherSO restartGameSO;

    public void RestartGame()
    {
        restartGameSO.RaiseEvent();
    }
}
