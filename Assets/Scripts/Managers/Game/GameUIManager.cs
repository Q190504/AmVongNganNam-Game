using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("In Game Stats")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI comboText;
    [SerializeField] private Slider healthBar; // Health Bar Slider

    [Header("Pause Panel")]
    [SerializeField] private GameObject pausePanel;

    [Header("End Panel")]
    [SerializeField] private GameObject endPanel;
    [SerializeField] private GameObject endPanel2;
    [SerializeField] private TMP_Text endScoreText;
    [SerializeField] private TMP_Text endComboText;
    [SerializeField] private TMP_Text completionStateText;
    [SerializeField] private TMP_Text totalPerfectText;
    [SerializeField] private TMP_Text totalGoodText;
    [SerializeField] private TMP_Text totalBadText;
    [SerializeField] private TMP_Text totalMissText;
    [SerializeField] private TMP_Text rankText;

    [SerializeField] private TMP_Text songTokenText;
    [SerializeField] private TMP_Text instTokenText;
    [SerializeField] private TMP_Text newRecordText;

    private void Start()
    {
        pausePanel.SetActive(false);
        HideEndPanel();
    }

    public void UpdateScoreUI(int newScore)
    {
        if (scoreText != null)
            scoreText.text = $"{newScore}";
        if (endScoreText != null) 
            endScoreText.text = newScore.ToString();
    }

    public void UpdateComboUI(int newCombo)
    {
        if (comboText != null)
            comboText.text = newCombo > 0 ? $"{newCombo}" : "0";
    }

    public void UpdateHealthUI(float newHealth)
    {
        if (healthBar != null)
        {
            SetHealthBar(newHealth);
        }
    }

    public void SetHealthBar(float health)
    {
        healthBar.value = health;
    }

    public void TogglePausePanel()
    {
        pausePanel.SetActive(PauseManager.IsPause);
    }

    public void ShowEndPanel()
    {
        //TO DO: CHECK IF FULL COMBO -> SET FULL COMBO TEXT TO ACTIVE
        endPanel.SetActive(true);
        endPanel2.SetActive(true);
        
    }

    public void ShowNewRecord()
    {
        newRecordText.gameObject.SetActive(true);
    }
    public void SetCompletionState(string completionState)
    {
        completionStateText.text = completionState.ToString();
    }

    public void SetHighestCombo(int newCombo)
    {
        endComboText.text = newCombo.ToString();
    }

    public void SetInstToken(int instToken)
    {
        instTokenText.text = "+" + instToken.ToString();
    }

    public void SetSongToken(int songToken)
    {
        songTokenText.text = "+" + songToken.ToString();
    }

    public void SetPerfectCount(int perfectCount)
    {
        totalPerfectText.text = perfectCount.ToString();
    }

    public void SetGoodCount(int goodCount)
    {
        totalGoodText.text = goodCount.ToString();
    }

    public void SetBadCount(int badCount)
    {
        totalBadText.text = badCount.ToString();
    }

    public void SetMissCount(int missionCount)
    {
        totalMissText.text = missionCount.ToString();
    }

    public void SetRank(string rank)
    {
        rankText.text = rank;
    }

    public void HideEndPanel()
    {
        endPanel.SetActive(false);
        endPanel2.SetActive(false);
        newRecordText.gameObject.SetActive(false);
    }
}
