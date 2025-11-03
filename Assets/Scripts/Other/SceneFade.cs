using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneFade : MonoBehaviour
{
    [Header("Fade Settings")]
    public Image imageToFade;
    public float fadeTime = 1f;

    private void Start()
    {
        if (imageToFade != null)
            StartCoroutine(FadeCoroutine(Color.black, Color.clear, null));
    }

    public void LoadLevel(int buildIndex)
    {
        if (imageToFade != null)
            StartCoroutine(FadeCoroutine(Color.clear, Color.black, () =>
            {
                Time.timeScale = 1f;
                SceneManager.LoadScene(buildIndex);
            }));
        else
            Time.timeScale = 1f;
        SceneManager.LoadScene(buildIndex);
    }

    private IEnumerator FadeCoroutine(Color _from, Color _to, Action _onEnd)
    {
        float time = 0f;
        while (time <= fadeTime)
        {
            time += Time.deltaTime;
            imageToFade.color = Color.Lerp(_from, _to, time / fadeTime);
            yield return null;
        }
        _onEnd?.Invoke();
    }
}
