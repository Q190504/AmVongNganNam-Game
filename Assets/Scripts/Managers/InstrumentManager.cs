using TMPro;
using UnityEngine;
using System.Collections.Generic;


public class InstrumentManager : MonoBehaviour
{
    public static InstrumentManager _instance;

    public List<InstrumentDataSO> instrumentList;
    private InstrumentDataSO currentInstrument;

    [SerializeField] private float fadeDuration;


    [SerializeField] TMP_Text currentInstrumentName;


    public static InstrumentManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindAnyObjectByType<InstrumentManager>();
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Debug.Log("Found more than one Instrument Manager in the scene. Destroying the newest one");
            Destroy(this.gameObject);
        }
    }

    public void SelectInstrument(int index)
    {
        if (index >= 0 && index < instrumentList.Count)
        {
            currentInstrument = instrumentList[index];
            currentInstrumentName.text = currentInstrument.name;
        }
    }

    public string GetInstrumentName(int index)
    {
        if (index >= 0 && index < instrumentList.Count)
            return instrumentList[index].instrumentName;
        else return "null";
    }

    public AudioClip GetNoteAudio(int noteIndex)
    {
        if (currentInstrument != null && noteIndex >= 0 && noteIndex < currentInstrument.notes.Length)
        {
            return currentInstrument.notes[noteIndex];
        }
        return null;
    }

    public float GetFadeDuration() { return fadeDuration; }
}
