using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class WinScreen : MonoBehaviour
{
    public GameObject winUI;
    public static bool isWinActive = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
        Time.timeScale = 0f;
            UnlockNextLevel();
            winUI.SetActive(true);
            isWinActive = true;

        }
    }

    private void UnlockNextLevel()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

        if (unlockedLevel <= currentIndex)
        {
            PlayerPrefs.SetInt("UnlockedLevel", currentIndex + 1);
            PlayerPrefs.Save();
            Debug.Log("Feloldott szint: " + (currentIndex + 1));
        }
    }

    private void OnDisable()
    {
        isWinActive = false;
    }
}


