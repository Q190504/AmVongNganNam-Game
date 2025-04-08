using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewInstrument", menuName = "Music/Instrument Data")]
public class InstrumentDataSO : ScriptableObject
{
    [SerializeField]
    private bool _isDefault;
    public bool isDefault => _isDefault;

    [SerializeField]
    private string _instrumentId;

    public string instrumentId => _instrumentId;

    [SerializeField]
    private string _instrumentName;

    public string instrumentName => _instrumentName;

    [SerializeField]
    private string _instrumentInfo;

    public string instrumentInfo => _instrumentInfo;


    [SerializeField]
    private Sprite _instrumentImage;

    public Sprite instrumentImage => _instrumentImage;

    [Header("Notes\n0 -> 6: C5 -> B5, 7 -> 13: C4 -> B4, 14 -> 20: C3 -> B3")]
    [SerializeField]
    private AudioClip[] _notes;
    //0->6: C5 -> B5
    //7->13: C4 -> B4
    //14->20: C3 -> B3

    public AudioClip[] notes => _notes;
}
