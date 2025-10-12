using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject gameOverScreen;
    public float delayBeforeStop = 1f;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void GameOver ()
    {
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
        }
        StartCoroutine(StopGameWithDelay());
    }

    public void RestartLevel ()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadNextLevel(){
        Time.timeScale = 1f;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int totalScenes = SceneManager.sceneCountInBuildSettings;

        if (currentSceneIndex + 1 < totalScenes)
        {
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
        else
        {
            SceneManager.LoadScene("Main Menu");
        }
    }

    public void QuitToMainMenu ()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }

    private IEnumerator StopGameWithDelay()
    {
        yield return new WaitForSeconds(delayBeforeStop);
        Time.timeScale = 0f;
    }
}
