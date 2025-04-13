using UnityEngine;

public class SwitchSceneButton : BaseButton
{
    public StringPublisherSO SwitchSceneSO;

    public void SwitchScene()
    {
        SwitchSceneButtonClick();
        SwitchSceneSO.RaiseEvent("");
    }
}
