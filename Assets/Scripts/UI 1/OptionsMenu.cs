using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    public GameObject bg;
    public RectTransform popupRect;
    public float scaleTime = 1f;

    public void Open()
    {
        bg.SetActive(true);
        StartCoroutine(ScaleCoroutine(0, 4, () =>
        {

        }));
    }

    public void Close()
    {
        StartCoroutine(ScaleCoroutine(4, 0, () =>
        {
            bg.SetActive(false);
        }));
    }

    public IEnumerator ScaleCoroutine(float _from, float _to, Action _onEnd)
    {
        float time = 0f;
        while (time < scaleTime)
        {
            time += Time.deltaTime;
            popupRect.localScale = Vector3.one * Mathf.Lerp(_from, _to, time / scaleTime);
            yield return null;
        }
        popupRect.localScale = Vector3.one * _to;
        _onEnd?.Invoke();
    }
}
