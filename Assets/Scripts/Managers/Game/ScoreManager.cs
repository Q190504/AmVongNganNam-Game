using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int score = 0;
    private int longestCombo = 0;
    private int combo = 0;
    private int perfect = 0;
    private int good = 0;
    private int bad = 0;
    private int miss = 0;
    private ConfigSO.CompletionState completionState = ConfigSO.CompletionState.NOT_COMPLETED;
    private ConfigSO.CompletionRank rank = ConfigSO.CompletionRank.F;

    [SerializeField] private ConfigSO config;
    [SerializeField] private IntPublisherSO healthEventPublisher;
    [SerializeField] private IntPublisherSO scoreEventPublisher;
    [SerializeField] private IntPublisherSO comboEventPublisher;
    [SerializeField] private IntPublisherSO finalComboEventPublisher;
    [SerializeField] private IntPublisherSO perfectEventPublisher;
    [SerializeField] private IntPublisherSO goodEventPublisher;
    [SerializeField] private IntPublisherSO badEventPublisher;
    [SerializeField] private IntPublisherSO missEventPublisher;
    [SerializeField] private StringPublisherSO rankEventPublisher;
    [SerializeField] private StringPublisherSO competionStateEventPublisher;
    [SerializeField] private VoidPublisherSO toggleNewRecordEventPublisher;
    private void Start()
    {
        config = SongManager.Instance.GetConfig();
        scoreEventPublisher.RaiseEvent(score);
        comboEventPublisher.RaiseEvent(combo);
        perfectEventPublisher.RaiseEvent(perfect);
        goodEventPublisher.RaiseEvent(good);
        badEventPublisher.RaiseEvent(bad);
        missEventPublisher.RaiseEvent(miss);
    }

    public void EvaluateHit(GameObject note)
    {
        GameNote gameNote = note.GetComponent<GameNote>();
        if (gameNote == null) return;

        switch (gameNote.GetHitResult())
        {
            case GameNote.HitResult.PERFECT:
                AddScore(config.perfectScore, true);
                perfect++;
                perfectEventPublisher.RaiseEvent(perfect);
                Debug.Log($"perfect {perfect}");
                break;
            case GameNote.HitResult.GOOD:
                AddScore(config.goodScore, true);
                good++;
                goodEventPublisher.RaiseEvent(good);
                break;
            case GameNote.HitResult.BAD:
                AddScore(config.badScore, false);
                bad++;
                badEventPublisher.RaiseEvent(bad);
                ClearCombo();
                healthEventPublisher.RaiseEvent(config.badPenalty);
                break;
            case GameNote.HitResult.MISS:
                ClearCombo();
                miss++;
                missEventPublisher.RaiseEvent(miss);
                healthEventPublisher.RaiseEvent(config.missPenalty);
                break;
        }
    }

    private void AddScore(int baseScore, bool applyComboBonus)
    {
        if (applyComboBonus)
        {
            // Combo Scaling Formula
            float comboMultiplier = 1 + (combo / (float)config.comboBonus);

            int finalScore = Mathf.RoundToInt(baseScore * comboMultiplier);
            score += finalScore;
            combo++;
            if (combo > longestCombo)
            {
                longestCombo = combo;
            }
        }
        scoreEventPublisher.RaiseEvent(score);
        comboEventPublisher.RaiseEvent(combo);
        Debug.Log($"Score: {score}, Combo: {combo}");
    }

    private void ClearCombo()
    {
        combo = 0;
        longestCombo = 0;
        comboEventPublisher.RaiseEvent(combo);
    }

    public int GetScore() => score;
    public int GetCombo() => combo;

    public int GetLongestCombo() => combo;

    private void ClearScore()
    {
        score = 0;
        scoreEventPublisher.RaiseEvent(score);
    }


    public void RestartGame()
    {
        ClearScore();
        ClearCombo();
        ClearNoteStats();
    }

    public void ClearNoteStats()
    {
        perfect = 0;
        good = 0;
        bad = 0;
        miss = 0;
    }

    public void EndGame(bool isCompleted)
    {
        Debug.Log(isCompleted);

        if (!isCompleted)
        {
            rankEventPublisher.RaiseEvent(rank.ToString());
            finalComboEventPublisher.RaiseEvent(longestCombo);
            competionStateEventPublisher.RaiseEvent(completionState.ToString());
            return;
        }

        

        var (songInfo, gameMode) = SongManager.Instance.GetCurrentSelectedSong();
        ScoreDataSO currentRecord = SongManager.Instance.GetScoreDataBySongID(songInfo.id);
        rank = config.calculateRank(score, songInfo, gameMode);

        completionState = ConfigSO.CompletionState.COMPLETED;
        int songToken = score / config.completion_token_div;
        int instrumentToken = 0;
        bool isChanged = false;

        bool isFullCombo = false;
        bool isAllPerfect = false;

        int noteCount = (gameMode == GameManager.GameMode.NORMAL) ?
            songInfo.easyNoteTimings.Count : songInfo.hardNoteTimings.Count;

        if (longestCombo == noteCount)
        {
            isFullCombo = true;
            if (perfect == longestCombo)
                isAllPerfect = true;
        }

        if (isAllPerfect)
            completionState = ConfigSO.CompletionState.ALL_PERFECT;
        else if (isFullCombo)
            completionState = ConfigSO.CompletionState.FULL_COMBO;

        if (gameMode == GameManager.GameMode.NORMAL)
        {
            if (currentRecord.easyScore < score)
            {
                currentRecord.easyScore = score;
                toggleNewRecordEventPublisher.RaiseEvent();

                isChanged = true;
            }

            instrumentToken = GetTokenReward(currentRecord.easyState, completionState);

            if (currentRecord.easyState < completionState)
            {
                currentRecord.easyState = completionState;
                isChanged = true;
            }
        }
        else // HARD mode
        {
            if (currentRecord.hardScore < score)
            {
                currentRecord.hardScore = score;
                toggleNewRecordEventPublisher.RaiseEvent();

                isChanged = true;
            }

            instrumentToken = GetTokenReward(currentRecord.hardState, completionState);

            if (currentRecord.hardState < completionState)
            {
                currentRecord.hardState = completionState;
                isChanged = true;
            }
        }

        GameDataSO gameData = SongManager.Instance.GetGameData();
        gameData.song_token += songToken;
        gameData.instrument_token += instrumentToken;

        if (isChanged) //If song record changed
        {
            ScoreInfo scoreInfo = new ScoreInfo
            {
                song_id = songInfo.id,
                easyScore = currentRecord.easyScore,
                easyState = ConfigSO.mapStateToString(currentRecord.easyState),
                hardScore = currentRecord.hardScore,
                hardState = ConfigSO.mapStateToString(currentRecord.hardState)
            };

            GameDataStorage.Instance.SaveGameStatus(
                null, null, new[] { scoreInfo },
                gameData.song_token, gameData.instrument_token
            );
        }
        else
        {
            GameDataStorage.Instance.SaveGameStatus(
                null, null, null,
                gameData.song_token, gameData.instrument_token
            );
        }

        finalComboEventPublisher.RaiseEvent(longestCombo);
        rankEventPublisher.RaiseEvent(rank.ToString());
        competionStateEventPublisher.RaiseEvent(completionState.ToString());
    }


    private int GetTokenReward(ConfigSO.CompletionState oldState, ConfigSO.CompletionState newState)
    {
        if (newState <= oldState)
            return 0;

        bool wasBelowFC = oldState < ConfigSO.CompletionState.FULL_COMBO;
        bool wasBelowAP = oldState < ConfigSO.CompletionState.ALL_PERFECT;

        int fc_token = SongManager.Instance.GetConfig().fc_token;
        int ap_token = SongManager.Instance.GetConfig().ap_token;

        if (newState == ConfigSO.CompletionState.FULL_COMBO && wasBelowFC)
            return fc_token;

        if (newState == ConfigSO.CompletionState.ALL_PERFECT && wasBelowAP)
        {
            return oldState <= ConfigSO.CompletionState.COMPLETED ? fc_token + ap_token : ap_token;
        }

        return 0;
    }

}
