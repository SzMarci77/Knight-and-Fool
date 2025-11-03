using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class LeverTest
{
    private GameObject leverObj;
    private Lever lever;
    private GameObject interactText;
    private GameObject wall;

    [SetUp]
    public void Setup()
    {
        leverObj = new GameObject("Lever");
        lever = leverObj.AddComponent<Lever>();

        interactText = new GameObject("Text");
        interactText.SetActive(true);
        wall = new GameObject("Wall");

        lever.interactText = interactText;
        lever.secretWallTileMap = wall;

        typeof(Lever)
            .GetMethod("Start", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.Invoke(lever, null);

    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(leverObj);
        Object.DestroyImmediate(interactText);
        Object.DestroyImmediate(wall);
    }

    [Test]
    public void Start_HidesInteractText()
    {
        Assert.IsFalse(interactText.activeSelf);
    }

    [Test]
    public void OnTriggerEnter2D_SetsPlayerInRangeTrue()
    {
        var player = new GameObject("Player");
        player.tag = "Player";
        var collider = player.AddComponent<BoxCollider2D>();

        // reflection -> közvetlenül hívjuk meg a privát Unity metódust
        typeof(Lever)
            .GetMethod("OnTriggerEnter2D", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.Invoke(lever, new object[] { collider });

        bool inRange = (bool)typeof(Lever)
            .GetField("playerInRange", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(lever);

        Assert.IsTrue(inRange);
        Object.DestroyImmediate(player);
    }

    [Test]
    public void OnTriggerExit2D_SetsPlayerInRangeFalse_AndHidesText()
    {
        var player = new GameObject("Player");
        player.tag = "Player";
        var collider = player.AddComponent<BoxCollider2D>();

        typeof(Lever)
            .GetField("playerInRange", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(lever, true);

        lever.interactText.SetActive(true);

        typeof(Lever)
            .GetMethod("OnTriggerExit2D", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.Invoke(lever, new object[] { collider });

        bool inRange = (bool)typeof(Lever)
            .GetField("playerInRange", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(lever);

        Assert.IsFalse(inRange, "PlayerInRange should be false after exiting trigger");
        Assert.IsFalse(lever.interactText.activeSelf, "Interact text should be hidden after exit");

        Object.DestroyImmediate(player);
    }
}
