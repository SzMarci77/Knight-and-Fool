using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsMenu : MonoBehaviour
{
    public AudioSource backgroundMusic;
    public Slider volumeSlider;
    public Text volumeText;
    public Toggle fullScToggle;

    void Start()
    {
        fullScToggle.isOn = Screen.fullScreen;

        fullScToggle.onValueChanged.AddListener(SetFullscreen);

        if (volumeSlider == null || volumeText == null)
        {
            Debug.LogWarning("Hiányzó UI komponens az OptionsMenu scriptben.");
            return;
        }

        float volume = PlayerPrefs.HasKey("Volume") ? PlayerPrefs.GetFloat("Volume") : 1f;

        if (backgroundMusic != null)
        {
            backgroundMusic.volume = volume;
        }

        volumeSlider.value = volume;
        volumeSlider.onValueChanged.AddListener(SetVolume);
        UpdateVolumeText(volume);
    }

    public void SetVolume(float volume)
    {
        if (backgroundMusic != null)
        {
            backgroundMusic.volume = volume;
        }

        PlayerPrefs.SetFloat("Volume", volume);
        PlayerPrefs.Save();
        UpdateVolumeText(volume);
    }

    private void UpdateVolumeText(float volume)
    {
        int volumePercentage = Mathf.RoundToInt(volume * 100f);
        volumeText.text = volumePercentage + "%";
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}
