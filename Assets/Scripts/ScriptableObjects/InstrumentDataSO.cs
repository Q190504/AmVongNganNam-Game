using UnityEngine;

[CreateAssetMenu(fileName = "NewInstrument", menuName = "Music/Instrument Data")]
public class InstrumentDataSO : ScriptableObject
{
    [Header("Instrument Name")]
    public string instrumentName;

    [Header("Notes\n0 -> 6: C4 -> B4, 7 -> 13: C3 -> B3, 14 -> 20: C2 -> B2")]
    public AudioClip[] notes;
    //0->6: C4 -> B4
    //7->13: C3 -> B3
    //14->20: C2 -> B2
}
