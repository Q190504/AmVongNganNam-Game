using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewScoreData", menuName = "RhythmGame/ScoreData")]

public class ScoreDataSO : ScriptableObject
{
    public string song_id;
    public int easyScore;
    public string easyState;
    public int hardScore;
    public string hardState;
}