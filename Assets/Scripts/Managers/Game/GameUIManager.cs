using TheHeroesJourney;
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

    private void Start()
    {
        pausePanel.SetActive(false);
    }

    public void UpdateScoreUI(int newScore)
    {
        if (scoreText != null)
            scoreText.text = $"Score: {newScore}";
    }

    public void UpdateComboUI(int newCombo)
    {
        if (comboText != null)
            comboText.text = newCombo > 0 ? $"Combo: {newCombo}" : "Combo: 0";
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
}
