﻿using System.Collections.Generic;
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
    [Header("Price")]
    public int songPrice = 5000;
    public int instPrice = 5;


}
