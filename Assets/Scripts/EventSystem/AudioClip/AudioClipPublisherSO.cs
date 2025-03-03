using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Audio Pulisher", menuName = "Scriptable Objects/Events/Audio Publisher")]
public class AudioClipPublisherSO : ScriptableObject
{
    public UnityAction<AudioClip> OnEventRaised;

    public void RaiseEvent(AudioClip obj)
    {
        OnEventRaised?.Invoke(obj);
    }
}