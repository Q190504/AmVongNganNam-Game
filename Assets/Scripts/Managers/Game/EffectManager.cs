using UnityEngine;

public class EffectManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem puffEffectPrefab;

    public void OnNoteDisappear(GameObject note)
    {
        if (note == null) return;

        Vector3 position = note.transform.position;

        ParticleSystem puff = Instantiate(puffEffectPrefab, position, puffEffectPrefab.transform.rotation);
        puff.Play();

        Destroy(puff.gameObject, puff.main.duration + puff.main.startLifetime.constantMax);
    }
}
