using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGameData", menuName = "RhythmGame/GameData")]

public class GameDataSO : ScriptableObject
{
    public List<string> unlocked_songs;
    public List<string> unlocked_instruments;
    public List<ScoreDataSO> highscore;
    public int song_token;
    public int instrument_token;
}
