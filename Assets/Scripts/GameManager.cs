using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Coins")]
    public int coinsCollected = 0;
    public int coinsToUnlockLevel = 10;
    public TMPro.TextMeshProUGUI coinText;

    [Header("Next Level")]
    public string nextSceneName;

    [Header("Game Over")]
    public GameObject gameOverPanel;

    void Awake()
    {
        // Singleton para acceder desde otros scripts
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddCoin()
    {
        coinsCollected++;
        UpdateUI();

        if (coinsCollected >= coinsToUnlockLevel)
        {
            UnlockNextLevel();
        }
    }

    private void UpdateUI()
    {
        if (coinText != null)
        {
            coinText.text = "Coins: " + coinsCollected + "/" + coinsToUnlockLevel;
        }
    }

    private void UnlockNextLevel()
    {
        Debug.Log("¡Nivel desbloqueado!");
        SceneManager.LoadScene(nextSceneName);
    }

    public void ShowGameOver()
    {
        if (gameOverPanel != null)
        {
            Time.timeScale = 0f; // Pausar el juego
            gameOverPanel.SetActive(true);
        }
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopAllSFX(); // Detener sonidos SFX
            AudioManager.Instance.PlayGameplayMusic();
        }

        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentIndex >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogWarning("No hay más escenas para cargar.");
            return;
        }

        SceneManager.LoadScene(currentIndex);
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopAllSFX();
            AudioManager.Instance.PlayMenuMusic();
        }

        SceneManager.LoadScene("MainMenu");
    }
}