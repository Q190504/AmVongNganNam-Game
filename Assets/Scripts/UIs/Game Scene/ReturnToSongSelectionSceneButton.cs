using UnityEngine;

public class ReturnToSongSelectionSceneButton : MonoBehaviour
{
    [SerializeField] private StringPublisherSO returnToSongSelectionSceneSO;

    public void ReturnToSongSelectionScene()
    {
        returnToSongSelectionSceneSO.RaiseEvent("");
    }
}
