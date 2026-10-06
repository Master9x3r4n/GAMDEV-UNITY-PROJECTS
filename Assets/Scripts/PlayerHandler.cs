using System;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private PlayerStats stats;
    
    void Start()
    {
        ResetPlayer();
    }

    private void OnEnable()
    {
        throw new NotImplementedException();
    }

    void ResetPlayer()
    {
        transform.position = stats.origin;
        stats.currentHealth = stats.maxHealth;
        stats.level = 1;
    }

    public void ReduceHealth()
    {
        //stats.currentHealth -= 1;
        Debug.Log("Player currentHealth: " + stats.currentHealth);
        
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

    private void TriggerGameOver()
    {
        FindAnyObjectByType<GameManager>().TriggerGameOver();
        ResetPlayer();
        Debug.Log("Game Over!");
    }
}
