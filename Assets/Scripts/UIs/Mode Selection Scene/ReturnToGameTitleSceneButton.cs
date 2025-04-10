using UnityEngine;

public class ReturnToGameTitleSceneButton : MonoBehaviour
{
    [SerializeField] private StringPublisherSO returnToGameTitleSceneSO;

    public void ReturnToGameTitleScene()
    {
        returnToGameTitleSceneSO.RaiseEvent("");
    }

}
