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
        if(SceneManager.GetActiveScene().buildIndex >= PlayerPrefs.GetInt("ReachedIndex"))
        {
            PlayerPrefs.SetInt("ReachedIndex", SceneManager.GetActiveScene().buildIndex + 1);
            PlayerPrefs.SetInt("UnlockedLevel", PlayerPrefs.GetInt("UnlockedLevel", 1) + 1);
            PlayerPrefs.Save();
        }
    }

    private void OnDisable()
    {
        isWinActive = false;
    }
}


