using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    public Image imageToFade;
    public float fadeTime = 1f;

    private void Start()
    {
        StartCoroutine(FadeCoroutine(Color.black, Color.clear, null));
    }

    public void LoadLevel(int levelIndex)
    {
        StartCoroutine(FadeCoroutine(Color.clear, Color.black, () =>
        {
            SceneManager.LoadSceneAsync(levelIndex);
        }));
        
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
