using UnityEngine;

public class ContinueButton : BaseButton
{
    [SerializeField] private VoidPublisherSO continueGameSO;

    public void ContinueGame()
    {
        ButtonClick();
        continueGameSO.RaiseEvent();
    }
}
