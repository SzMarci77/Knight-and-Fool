using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game State")]
    public bool isGameOver = false;
    public int score = 0;

    [Header("Events")]
    public UnityEvent onGameOver;
    public UnityEvent onGameRestart;
    public UnityEvent<int> onScoreChanged;

    private void Awake()
    {
        // Singleton biztosítás
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        // Debug teszt: R billentyűvel újraindít
        if (isGameOver && Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }

    // Meghívható ha a játékos meghal
    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        Debug.Log("Game Over!");
        onGameOver?.Invoke();
        Time.timeScale = 0f;
    }

    // Újraindítás
    public void RestartGame()
    {
        isGameOver = false;
        score = 0;

        Debug.Log("Restarting game...");
        onGameRestart?.Invoke();

        // Jelenlegi pálya újratöltése
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Pont hozzáadása
    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log("Pont: " + score);
        onScoreChanged?.Invoke(score);
    }
}
