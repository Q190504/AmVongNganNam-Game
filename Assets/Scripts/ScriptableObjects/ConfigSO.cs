using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewConfig", menuName = "RhythmGame/Config")]
public class ConfigSO : ScriptableObject
{
    public float BeatsShownOnScreen;
    public float TapAccuracy = 0.0f;
    [Header("Scoring Bands")]
    public int perfectScore = 100;
    public int goodScore = 50;
    public int badScore = 20;
    public int comboBonus = 10;
    [Header("Scoring Thresholds (Seconds)")]
    public float perfectThreshold = 0.1f;
    public float goodThreshold = 0.3f;
    public float badThreshold = 0.5f;
    [Header("Penalty")]
    public int badPenalty = 5;
    public int missPenalty = 10;
    [Header("Money")]
    public int fc_token = 1;
    public int ap_token = 2;
    public int completion_token_div = 500;
    [Header("Price")]
    public int songPrice = 1500;
    public int instPrice = 5;

    public enum CompletionState
    {
        NOT_COMPLETED,
        COMPLETED,
        FULL_COMBO,
        ALL_PERFECT
    };

    public enum CompletionRank
    {
        F,
        D,
        C,
        B,
        A,
        S
    };

    public CompletionRank calculateRank (int score, SongInfoSO song, GameManager.GameMode mode)
    {
        int numNotes = mode == GameManager.GameMode.NORMAL ? song.easyNoteTimings.Count : song.hardNoteTimings.Count;

        int maxScore = numNotes * perfectScore;
        float percentage = (float)score / maxScore;

        if (percentage >= 0.95f)
            return CompletionRank.S;
        if (percentage >= 0.85f)
            return CompletionRank.A;
        if (percentage >= 0.75f)
            return CompletionRank.B;
        if (percentage >= 0.60f)
            return CompletionRank.C;
        
        return CompletionRank.D;
    }

    private static Dictionary<string, CompletionState> stateMap = new Dictionary<string, CompletionState>()
    {
        { "NC", CompletionState.NOT_COMPLETED },
        { "C", CompletionState.COMPLETED },
        { "FC", CompletionState.FULL_COMBO },
        { "AP", CompletionState.ALL_PERFECT }
    };

    private static Dictionary<CompletionState, string> reversedtateMap = new Dictionary<CompletionState, string>()
    {
        { CompletionState.NOT_COMPLETED, "NC" },
        { CompletionState.COMPLETED, "C" },
        { CompletionState.FULL_COMBO, "FC" },
        { CompletionState.ALL_PERFECT, "AP" }
    };

    public static CompletionState mapStringToState (string stateString)
    {
        return (stateMap[stateString]);
    }

    public static string mapStateToString(CompletionState state)
    {
        return (reversedtateMap[state]);
    }

}
