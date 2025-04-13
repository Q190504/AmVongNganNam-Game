using UnityEngine;

public class ReturnToGameTitleSceneButton : BaseButton
{
    [SerializeField] private StringPublisherSO returnToGameTitleSceneSO;

    public void ReturnToGameTitleScene()
    {
        SwitchSceneButtonClick();
        returnToGameTitleSceneSO.RaiseEvent("");
    }

}
