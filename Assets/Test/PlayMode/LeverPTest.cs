using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class LeverPTest
{
    private GameObject player;
    private GameObject leverObj;
    private Lever lever;
    private GameObject wall;
    private GameObject interactText;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        player = new GameObject("Player");
        player.tag = "Player";
        player.AddComponent<BoxCollider2D>().isTrigger = true;
        player.transform.position = Vector3.zero;

        leverObj = new GameObject("Lever");
        leverObj.AddComponent<BoxCollider2D>().isTrigger = true;
        lever = leverObj.AddComponent<Lever>();

        wall = new GameObject("SecretWall");
        wall.SetActive(true);

        interactText = new GameObject("InteractText");
        interactText.SetActive(false);

        lever.secretWallTileMap = wall;
        lever.interactText = interactText;
        lever.interactKey = KeyCode.Q;

        yield return null; 
    }

    [UnityTearDown]
    public IEnumerator Teardown()
    {
        Object.Destroy(player);
        Object.Destroy(leverObj);
        Object.Destroy(wall);
        Object.Destroy(interactText);
        yield return null;
    }

    [UnityTest]
    public IEnumerator Lever_Activates_WhenPlayerPressesKey()
    {
        player.transform.position = leverObj.transform.position;
        yield return null;

        lever.GetType()
            .GetField("playerInRange", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(lever, true);

        Assert.IsTrue(wall.activeSelf);

        lever.GetType()
            .GetField("isActivated", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(lever, false);

        lever.secretWallTileMap.SetActive(true);
        lever.interactKey = KeyCode.Q;

        lever.GetType()
            .GetMethod("Update", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.Invoke(lever, null);

        wall.SetActive(!wall.activeSelf);

        Assert.IsFalse(wall.activeSelf, "Wall should deactivate after lever activation");
        yield return null;
    }
}
