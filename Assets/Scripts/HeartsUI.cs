using UnityEngine;
using UnityEngine.UI;

public class HeartsUI : MonoBehaviour
{
    [Header("UI Setup")]
    [SerializeField] private Image[] heartImages; 
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;

    private void OnEnable()
    {
        PlayerHandler.OnHealthChanged += UpdateHearts;
        GameManager.OnGameOver += HideHearts;
        GameManager.OnGameStart += ShowHearts;
    }

    private void OnDisable()
    {
        PlayerHandler.OnHealthChanged -= UpdateHearts;
        GameManager.OnGameOver -= HideHearts;
        GameManager.OnGameStart -= ShowHearts;
    }

    private void UpdateHearts(int currentHealth)
    {
        ShowHearts();

        for (int i = 0; i < heartImages.Length; i++)
        {
            if (heartImages[i] == null) continue;

            if (i < currentHealth)
            {
                heartImages[i].sprite = fullHeart;
            }
            else
            {
                heartImages[i].sprite = emptyHeart;
            }
        }
    }

    private void HideHearts()
    {
        foreach (Image heart in heartImages)
        {
            if (heart != null)
            {
                heart.gameObject.SetActive(false);
            }
        }
    }

    private void ShowHearts()
    {
        foreach (Image heart in heartImages)
        {
            if (heart != null)
            {
                heart.gameObject.SetActive(true);
            }
        }
    }
}