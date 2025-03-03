using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class Note : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private int noteIndex;

    private float fadeDuration;
    private AudioSource audioSource;
    private Coroutine fadeCoroutine;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        fadeDuration = InstrumentManager.Instance.GetFadeDuration();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        AudioClip clip = InstrumentManager.Instance.GetNoteAudio(noteIndex);
        if (clip != null)
        {
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine); // Stop fade if ongoing
            }

            audioSource.volume = 1.0f; // Reset volume before playing
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (audioSource.isPlaying)
        {
            fadeCoroutine = StartCoroutine(FadeOutAudio());
        }
    }

    private IEnumerator FadeOutAudio()
    {
        float startVolume = audioSource.volume;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
            yield return null;
        }

        audioSource.volume = 0;
        audioSource.Stop();
    }
}
