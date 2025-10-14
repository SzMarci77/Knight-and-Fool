using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    [Header("UI Elements")]
    public GameObject pauseMenuUI;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip pauseOpenSound;
    [SerializeField] private AudioClip pauseCloseSound;
    [SerializeField] private AudioSource audioSource;

    private void Start()
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        if(WinScreen.isWinActive)
        {
            return;
        }

        // Escape gomb lenyomás ->  folytatás
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;

        if (pauseCloseSound != null)
        {
            audioSource.PlayOneShot(pauseCloseSound);
        }
    }
    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;

        if( pauseOpenSound != null)
        {
            audioSource.PlayOneShot(pauseOpenSound);
        }
    }

    public void LoadMenu()
    {

        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
