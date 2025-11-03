using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class GameManagerTest
{
    private GameObject gmObj;
    private GameManager gameManager;
    private GameObject screenObj;

    [SetUp]
    public void Setup()
    {
        gmObj = new GameObject("GameManager");
        gameManager = gmObj.AddComponent<GameManager>();

        typeof(GameManager)
    .GetMethod("Awake", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
    ?.Invoke(gameManager, null);

        screenObj = new GameObject("GameOverScreen");
        screenObj.SetActive(false);

        gameManager.gameOverScreen = screenObj;
        gameManager.delayBeforeStop = 0.1f;
    }

    [TearDown]
    public void Teardown()
    {
        if (GameManager.Instance != null)
            Object.DestroyImmediate(GameManager.Instance.gameObject);

        Object.DestroyImmediate(screenObj);
        Object.DestroyImmediate(gmObj);
        Time.timeScale = 1f; // reset
    }

    [Test]
    public void Awake_SetsInstance()
    {
        Assert.AreSame(gameManager, GameManager.Instance);
    }

    [Test]
    public void GameOver_ActivatesGameOverScreen()
    {
        gameManager.GameOver();
        Assert.IsTrue(screenObj.activeSelf, "GameOverScreen should be active after GameOver()");
    }

    [Test]
    public void StopGameWithDelay_SetsTimeScaleToZero()
    {
        Time.timeScale = 1f;
        gameManager.delayBeforeStop = 0f;

        var enumerator = typeof(GameManager)
            .GetMethod("StopGameWithDelay", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.Invoke(gameManager, null) as IEnumerator;

        while (enumerator.MoveNext()) { }

        Assert.AreEqual(0f, Time.timeScale, "Time scale should be 0 after StopGameWithDelay()");
    }
}
