using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro; 

[RequireComponent(typeof(Slider))]
public class VolumeSlider : MonoBehaviour
{
    private Slider slider;

    public AudioMixer auMixer;
    public string volumeName;

    // Új: a hangerőszöveg
    public TextMeshProUGUI volumeText;

    private void Start()
    {
        slider = GetComponent<Slider>();

        var value = PlayerPrefs.GetFloat(volumeName, 1f); // alapértelmezett 1
        slider.value = value;
        SetVolume(value);

        slider.onValueChanged.AddListener(SetVolume);

        // Frissítjük a szöveget a startnál
        UpdateVolumeText(value);
    }

    public void SetVolume(float volume)
    {
        float dB = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20;
        auMixer.SetFloat(volumeName, dB);

        PlayerPrefs.SetFloat(volumeName, volume);

        // Szöveg frissítése
        UpdateVolumeText(volume);
    }

    private void UpdateVolumeText(float volume)
    {
        if (volumeText != null)
        {
            int volumePercentage = Mathf.RoundToInt(volume * 100f);
            volumeText.text = volumePercentage + "%";
        }
    }
}
