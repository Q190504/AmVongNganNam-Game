using UnityEngine;

public class ReturnToSongSelectionSceneButton : BaseButton
{
    [SerializeField] private StringPublisherSO returnToSongSelectionSceneSO;

    public void ReturnToSongSelectionScene()
    {
        SwitchSceneButtonClick();
        returnToSongSelectionSceneSO.RaiseEvent("");
    }
}
