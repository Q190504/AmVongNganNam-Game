using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int score = 0;
    private int combo = 0;

    [SerializeField] private ConfigSO config;
    [SerializeField] private IntPublisherSO healthEventPublisher;
    [SerializeField] private IntPublisherSO scoreEventPublisher;
    [SerializeField] private IntPublisherSO comboEventPublisher;

    private void Start()
    {
        scoreEventPublisher.RaiseEvent(score);
        comboEventPublisher.RaiseEvent(combo);
    }

    public void EvaluateHit(GameObject note)
    {
        GameNote gameNote = note.GetComponent<GameNote>();
        if (gameNote == null) return;

        switch (gameNote.GetHitResult())
        {
            case GameNote.HitResult.PERFECT:
                AddScore(config.perfectScore, true);
                break;
            case GameNote.HitResult.GOOD:
                AddScore(config.goodScore, true);
                break;
            case GameNote.HitResult.BAD:
                AddScore(config.badScore, false);
                ClearCombo();
                healthEventPublisher.RaiseEvent(config.badPenalty);
                break;
            case GameNote.HitResult.MISS:
                ClearCombo();
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
    }
}
