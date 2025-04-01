using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGameData", menuName = "RhythmGame/GameData")]

public class GameDataSO : ScriptableObject
{
    public string[] unlocked_songs;
    public string[] unlocked_instruments;
    public List<ScoreInfo> highscore;
}
