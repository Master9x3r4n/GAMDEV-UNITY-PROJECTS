using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerStats stats;
    public static event Action OnGameOver;
    public static event Action OnGameStart;
    public static event Action<int> LevelUp;
    

    public void TriggerGameOver()
    {
        Debug.Log("Game Over!");
        OnGameOver?.Invoke();
    }

    public void TriggerGameStart()
    {
        Debug.Log("Game Start!");
        OnGameStart?.Invoke();
    }

    public void TriggerLevelUp()
    {
        Debug.Log("Level Up!");
        LevelUp?.Invoke(stats.level);
    }
}
