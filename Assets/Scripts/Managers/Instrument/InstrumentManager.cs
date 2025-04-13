using TMPro;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;


public class InstrumentManager : MonoBehaviour
{
    public static InstrumentManager _instance;

    [SerializeField]
    private List<InstrumentDataSO> instrumentList;
    private InstrumentDataSO currentInstrument;

    public StringPublisherSO selectInstrumenPublisher;

    [SerializeField] private float fadeDuration;

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
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.Log("Found more than one Instrument Manager in the scene. Destroying the newest one");
            Destroy(this.gameObject);
        }
    }

    public List<InstrumentDataSO> GetInstrumentDatas()
    {
        return instrumentList;
    }

    public void SelectInstrument(string id)
    {
        currentInstrument = FindById(id);
        selectInstrumenPublisher.RaiseEvent(currentInstrument.instrumentName);
    }

    public InstrumentDataSO FindById(string id)
    {
        return instrumentList.Find(instrument => instrument.instrumentId == id);
    }
    public string GetInstrumentName(string id)
    {
        var inst = FindById(id);
        if (inst)
        {
            return inst.instrumentName;
        }
        return "null";
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
