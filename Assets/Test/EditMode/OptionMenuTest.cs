using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class OptionMenuTest
{
    private GameObject menuObj;
    private OptionsMenu menu;
    private GameObject bgObj;
    private RectTransform popupRect;

    [SetUp]
    public void Setup()
    {
        menuObj = new GameObject("OptionsMenu");
        menu = menuObj.AddComponent<OptionsMenu>();

        bgObj = new GameObject("BG");
        bgObj.SetActive(false);

        var popupGO = new GameObject("Popup", typeof(RectTransform));
        popupRect = popupGO.GetComponent<RectTransform>();

        menu.bg = bgObj;
        menu.popupRect = popupRect;
        menu.scaleTime = 0.1f;
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(menuObj);
        Object.DestroyImmediate(bgObj);
        Object.DestroyImmediate(popupRect.gameObject);
    }

    // --- 1. Teszt: Open() bekapcsolja a hátteret ---
    [Test]
    public void Open_ActivatesBackground()
    {
        menu.Open();
        Assert.IsTrue(menu.bg.activeSelf);
    }

    // --- 2. Teszt: Coroutine helyesen skáláz ---
    [UnityTest]
    public IEnumerator ScaleCoroutine_SetsFinalScaleProperly()
    {
        popupRect.localScale = Vector3.zero;

        yield return menu.ScaleCoroutine(0, 4, null);

        Assert.AreEqual(Vector3.one * 4, popupRect.localScale);
    }

    // --- 3. Teszt: Close() végén a háttér kikapcsol ---
    [UnityTest]
    public IEnumerator Close_DeactivatesBackgroundAtEnd()
    {
        menu.bg.SetActive(true);

        yield return menu.ScaleCoroutine(4, 0, () =>
        {
            menu.bg.SetActive(false);
        });

        Assert.IsFalse(menu.bg.activeSelf);
        Assert.AreEqual(Vector3.zero, popupRect.localScale);
    }
}
