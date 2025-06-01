using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem puffEffectPrefab;
    [SerializeField] private TextMeshProUGUI textEffect;
    public void OnNoteDisappear(GameObject note)
    {
        if (note == null) return;

        GameNote gameNote = note.GetComponent<GameNote>();
        if (gameNote == null) return;

        if (gameNote.GetHitResult() == GameNote.HitResult.NONE) return;

        Vector3 position = note.transform.position;

        textEffect.gameObject.SetActive(false);
        textEffect.text = gameNote.GetHitResult().ToString();
        textEffect.gameObject.SetActive(true);

        ParticleSystem puff = Instantiate(puffEffectPrefab, position, puffEffectPrefab.transform.rotation);
        puff.Play();

        Destroy(puff.gameObject, puff.main.duration + puff.main.startLifetime.constantMax);
    }
}
