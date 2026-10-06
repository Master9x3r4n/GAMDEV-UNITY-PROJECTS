using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameEndUI : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private PlayerStats stats;

    [Header("UI Elements")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Image panelImage;
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Start()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    private void OnEnable()
    {
        GameManager.OnGameOver += DisplayText;
    }

    private void OnDisable()
    {
        GameManager.OnGameOver -= DisplayText;
    }

    private void DisplayText()
    {
        if (stats == null)
        {
            Debug.LogWarning("PlayerStats reference is missing on GameEndUI!", this);
            return;
        }
        
        if (stats.currentHealth <= 0)
        {
            statusText.text = "YOU LOSE";
            panelImage.color = new Color32(255, 100, 0, 66);
        }
        else
        {
            statusText.text = "YOU ESCAPED!";
            panelImage.color = new Color32(160, 255, 0, 66);
        }
        scoreText.text = $"Your Score: {stats.score}";
        
        // Activate panel
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }
    
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}