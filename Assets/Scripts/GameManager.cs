using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    // public Text pointsText;
    public static GameManager Instance;
    public GameObject gameOverScreen;
    //Késleltetés
    public float delayBeforeStop = 1f;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    /*
    public void SetUp(int score)
    {
        gameObject.SetActive(true);
        pointsText.text = score.ToString() + " POINTS";
    }
    */
    public void GameOver ()
    {
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
        }
        // Játék csak késleltetéssel áll le
        StartCoroutine(StopGameWithDelay());
    }

    public void RestartLevel ()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitToMainMenu ()
    {
        SceneManager.LoadScene("Main Menu");
    }

    private IEnumerator StopGameWithDelay()
    {
        yield return new WaitForSeconds(delayBeforeStop);
        Time.timeScale = 0f;
    }
}
