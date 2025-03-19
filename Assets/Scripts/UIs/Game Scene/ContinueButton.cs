using UnityEngine;

public class ContinueButton : MonoBehaviour
{
    [SerializeField] private VoidPublisherSO continueGameSO;

    public void ContinueGame()
    {
        continueGameSO.RaiseEvent();
    }
}
