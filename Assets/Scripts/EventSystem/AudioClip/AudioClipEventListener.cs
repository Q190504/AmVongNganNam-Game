using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioClipEventListener : MonoBehaviour
{
    [SerializeField] private UnityEvent<AudioClip> EventResponse;
    [SerializeField] private AudioClipPublisherSO publisher;

    private void OnEnable()
    {
        publisher.OnEventRaised += Respond;
    }

    private void OnDisable()
    {
        publisher.OnEventRaised -= Respond;
    }

    private void Respond(AudioClip obj)
    {
        EventResponse?.Invoke(obj);
    }
}