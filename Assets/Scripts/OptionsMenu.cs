using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private AudioSource backgroundMusic;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TextMeshProUGUI volumeText;
    [SerializeField] private Toggle fullScToggle;


    void Start()
    {
        // Fullscreen alapbeállítás
        if(fullScToggle == null)
        {
            fullScToggle.isOn = Screen.fullScreen;
            fullScToggle.onValueChanged.AddListener(SetFullscreen);
        }


        if (volumeSlider == null || volumeText == null)
        {
            Debug.LogWarning("Hiányzó UI komponens az OptionsMenu scriptben.");
            return;
        }

        // Hangerő beállítása PlayerPrefs alapján, alapértelmezett érték 1.0 (100%)
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
