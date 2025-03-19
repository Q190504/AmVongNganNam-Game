using UnityEngine;

public class StartGameModeButton : MonoBehaviour
{
    [SerializeField] private StringPublisherSO switchToGameModeSceneSO;

    public void SwitchToGameModeScene()
    {
        switchToGameModeSceneSO.RaiseEvent("");
    }
}
