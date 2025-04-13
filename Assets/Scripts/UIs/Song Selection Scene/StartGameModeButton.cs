using UnityEngine;

public class StartGameModeButton : BaseButton
{
    [SerializeField] private StringPublisherSO switchToGameModeSceneSO;

    public void SwitchToGameModeScene()
    {
        SwitchToGameModeScene();
        switchToGameModeSceneSO.RaiseEvent("");
    }
}
