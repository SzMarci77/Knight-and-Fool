using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField]
    private SoundLibrary sfxLibrary;
    [SerializeField]
    private AudioSource sfx2DSource;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Hang lejátszása a név alapján
    public void PlaySound2D(string soundName)
    {
        if (sfxLibrary == null || sfx2DSource == null)
        {
            Debug.LogWarning("SoundManager: Hiányzó referencia az Inspectorban!");
            return;
        }

        AudioClip clip = sfxLibrary.GetClipFromName(soundName);
        if (clip != null)
        {
            sfx2DSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"SoundManager: '{soundName}' nem található a SoundLibrary-ban!");
        }
    }
}
