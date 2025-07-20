using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("SFX Settings")]
    public AudioSource sfxSource;
    public AudioClip coinClip;
    public AudioClip gameOverClip;

    [Header("Music Settings")]
    public AudioSource musicSource;
    public AudioClip gameplayMusic;
    public AudioClip menuMusic;

    [Range(0f, 1f)]
    public float volume = 1f;

    void Awake()
    {
        // Singleton global
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persiste entre escenas

            // Suscribirse al evento de carga de escenas
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Determinar qué música reproducir según la escena actual
        string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        if (currentScene == "MainMenu")
        {
            PlayMenuMusic();
        }
        else
        {
            PlayGameplayMusic();
        }
    }

    public void PlayCoinSound()
    {
        if (coinClip != null)
        {
            sfxSource.PlayOneShot(coinClip, volume);
        }
    }

    public void PlayGameOverSound()
    {
        if (gameOverClip != null)
        {
            sfxSource.PlayOneShot(gameOverClip, volume);
        }
    }

    // Nuevo método para detener todos los sonidos SFX
    public void StopAllSFX()
    {
        if (sfxSource != null)
        {
            sfxSource.Stop();
        }
    }

    // Reproducir música del menú principal
    public void PlayMenuMusic()
    {
        StopAllSFX();
        if (menuMusic != null && musicSource != null)
        {
            musicSource.clip = menuMusic;
            musicSource.loop = true;
            musicSource.volume = volume;
            musicSource.Play();
        }
    }

    // Reproducir música del gameplay
    public void PlayGameplayMusic()
    {
        StopAllSFX();
        if (gameplayMusic != null && musicSource != null)
        {
            musicSource.clip = gameplayMusic;
            musicSource.loop = true;
            musicSource.volume = volume;
            musicSource.Play();
        }
    }

    public void PlayBackgroundMusic()
    {
        string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        if (currentScene == "MainMenu")
        {
            PlayMenuMusic();
        }
        else
        {
            PlayGameplayMusic();
        }
    }

    public void StopBackgroundMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
        }
    }

    public void StopAllAudio()
    {
        StopBackgroundMusic();
        StopAllSFX();
    }

    // Método automático para cambiar música según la escena
    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        if (scene.name == "MainMenu")
        {
            PlayMenuMusic();
        }
        else
        {
            PlayGameplayMusic();
        }
    }

    void OnDestroy()
    {
        // Desuscribirse del evento para evitar memory leaks
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void SetVolume(float newVolume)
    {
        volume = Mathf.Clamp01(newVolume);
        if (musicSource != null)
            musicSource.volume = volume;
    }
}