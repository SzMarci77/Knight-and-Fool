using Cinemachine;
using System.Collections;
using UnityEngine;

public class ZoomInEffekt : MonoBehaviour
{
    public CinemachineVirtualCamera vcam;
    public float zoomAmount = 3f;   
    public float zoomDuration = 0.2f;
    public float zoomReturnTime = 0.3f; 

    private float originalOrthoSize;

    void Start()
    {
        if (vcam != null)
        {
            originalOrthoSize = vcam.m_Lens.OrthographicSize;
        }
    }

    public void TriggerZoom()
    {
        StopAllCoroutines();
        StartCoroutine(DoZoom());
    }

    private IEnumerator DoZoom()
    {
        // Zoom in
        float targetSize = originalOrthoSize - zoomAmount;
        float t = 0f;
        while (t < zoomDuration)
        {
            t += Time.deltaTime;
            vcam.m_Lens.OrthographicSize = Mathf.Lerp(originalOrthoSize, targetSize, t / zoomDuration);
            yield return null;
        }
        vcam.m_Lens.OrthographicSize = targetSize;

        // Zoom out
        t = 0f;
        while (t < zoomReturnTime)
        {
            t += Time.deltaTime;
            vcam.m_Lens.OrthographicSize = Mathf.Lerp(targetSize, originalOrthoSize, t / zoomReturnTime);
            yield return null;
        }
        vcam.m_Lens.OrthographicSize = originalOrthoSize;
    }
}