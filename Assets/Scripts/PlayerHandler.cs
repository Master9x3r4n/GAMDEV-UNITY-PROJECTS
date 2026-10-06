using System;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private PlayerStats stats;
    public static event Action<int> OnHealthChanged;
    
    void Start()
    {
        ResetPlayer();
    }

    void ResetPlayer()
    {
        transform.position = stats.origin;
        stats.currentHealth = stats.maxHealth;
        stats.level = 1;
        stats.score = 0;
        
        OnHealthChanged?.Invoke(stats.currentHealth);
    }

    public void ReduceHealth()
    {
        stats.currentHealth -= 1;
        Debug.Log("Player currentHealth: " + stats.currentHealth);
        
        OnHealthChanged?.Invoke(stats.currentHealth);
        
        if (stats.currentHealth <= 0)
        {
            TriggerGameOver();
        }
    }

    public void LevelUp()
    {
        stats.level++;
        FindAnyObjectByType<GameManager>().TriggerLevelUp();
    }

    public void TriggerGameOver()
    {
        FindAnyObjectByType<GameManager>().TriggerGameOver();
        Debug.Log("Game Over!");
    }

    public void IncreaseScore()
    {
        stats.score++;
    }
}
