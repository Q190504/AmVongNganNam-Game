using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int score = 0;
    private int combo = 0;
    private int perfect = 0;
    private int good = 0;
    private int bad = 0;
    private int miss = 0;

    private string rank = "NULL";

    [SerializeField] private ConfigSO config;
    [SerializeField] private IntPublisherSO healthEventPublisher;
    [SerializeField] private IntPublisherSO scoreEventPublisher;
    [SerializeField] private IntPublisherSO comboEventPublisher;
    [SerializeField] private IntPublisherSO perfectEventPublisher;
    [SerializeField] private IntPublisherSO goodEventPublisher;
    [SerializeField] private IntPublisherSO badEventPublisher;
    [SerializeField] private IntPublisherSO missEventPublisher;
    [SerializeField] private StringPublisherSO rankEventPublisher;

    private void Start()
    {
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
        }
        scoreEventPublisher.RaiseEvent(score);
        comboEventPublisher.RaiseEvent(combo);
        Debug.Log($"Score: {score}, Combo: {combo}");
    }

    private void ClearCombo()
    {
        combo = 0;
        comboEventPublisher.RaiseEvent(combo);
    }

    public int GetScore() => score;
    public int GetCombo() => combo;

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

    public void EndGame()
    {
        //TO DO: CALCULATE RANK
        rank = "A";

        rankEventPublisher.RaiseEvent(rank);
    }
}
