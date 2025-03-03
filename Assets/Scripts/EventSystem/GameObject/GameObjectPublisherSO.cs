using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Object Pulisher", menuName = "Scriptable Objects/Events/Object Publisher")]
public class GameObjectPublisherSO : ScriptableObject
{
    public UnityAction<GameObject> OnEventRaised;

    public void RaiseEvent(GameObject obj)
    {
        OnEventRaised?.Invoke(obj);
    }
}