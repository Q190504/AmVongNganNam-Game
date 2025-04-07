using UnityEngine;

public class HealthManager : MonoBehaviour
{
    private int health = 100; // Default health
    private int maxHealth = 100; // Default health
    [SerializeField] FloatPublisherSO healthChangePublisher;

    [SerializeField] VoidPublisherSO endGameEventPublisher;

    public void DecreaseHealth(int amount)
    {
        health -= amount;
        health = Mathf.Max(health, 0); // Prevent negative health
        healthChangePublisher.RaiseEvent((float)health/ maxHealth);

        Debug.Log($"Health Decreased! Current Health: {health}");

        if (health <= 0)
        {
            HandleGameOver();
        }
    }

    private void HandleGameOver()
    {
        Debug.Log("Game Over!");
        endGameEventPublisher.RaiseEvent();
    }

    public void ResetHealth()
    {
        health = maxHealth;
        healthChangePublisher.RaiseEvent((float)health / maxHealth);
    }
    public int GetHealth() => health;

    public void Restart()
    {
        ResetHealth();
    }
}
