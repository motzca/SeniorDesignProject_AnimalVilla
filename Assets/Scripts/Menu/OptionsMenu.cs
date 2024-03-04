using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private Button saveButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private Slider soundEffectsVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider hapticsSlider;
    [SerializeField] private GameObject startMenu;
    [SerializeField] private GameObject optionsMenu;
    private float initialSoundEffectsVolume;
    private float initialMusicVolume;
    private float initialHapticsIntensity;

    void Start()
    {
        saveButton.onClick.AddListener(() => {
            SaveSettings();
            SwitchToStartMenu();
        });
        cancelButton.onClick.AddListener(() => {
            CancelChanges();
            SwitchToStartMenu();
        });
        LoadCurrentSettings();
    }

    void LoadCurrentSettings()
    {
        initialSoundEffectsVolume = soundEffectsVolumeSlider.value = PlayerPrefs.GetFloat("SoundEffectsVolume", 100);
        initialMusicVolume = musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 100);
        initialHapticsIntensity = hapticsSlider.value = PlayerPrefs.GetFloat("HapticsIntensity", 100);
    }

    void SaveSettings()
    {
        PlayerPrefs.SetFloat("SoundEffectsVolume", soundEffectsVolumeSlider.value);
        PlayerPrefs.SetFloat("MusicVolume", musicVolumeSlider.value);
        PlayerPrefs.SetFloat("HapticsIntensity", hapticsSlider.value);
        PlayerPrefs.Save();
        initialSoundEffectsVolume = soundEffectsVolumeSlider.value;
        initialMusicVolume = musicVolumeSlider.value;
        initialHapticsIntensity = hapticsSlider.value;
    }

    void CancelChanges()
    {
        soundEffectsVolumeSlider.value = initialSoundEffectsVolume;
        musicVolumeSlider.value = initialMusicVolume;
        hapticsSlider.value = initialHapticsIntensity;
    }

    void SwitchToStartMenu()
    {
        optionsMenu.SetActive(false);
        startMenu.SetActive(true);
    }
}
