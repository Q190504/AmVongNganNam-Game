using UnityEngine;

public class SwitchSceneButton : MonoBehaviour
{
    public StringPublisherSO SwitchSceneSO;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchScene()
    {
        SwitchSceneSO.RaiseEvent("");
    }
}
