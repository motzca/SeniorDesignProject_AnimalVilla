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
    
    private AudioManager AudioManager; 

    private float initialSoundEffectsVolume; 
    private float initialMusicVolume;
    private float initialHapticsIntensity;

    void Start()
    {
        AudioManager = FindObjectOfType<AudioManager>();

        soundEffectsVolumeSlider.onValueChanged.AddListener(HandleSoundEffectsVolumeChanged);
        musicVolumeSlider.onValueChanged.AddListener(HandleMusicVolumeChanged);

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
        initialSoundEffectsVolume = soundEffectsVolumeSlider.value = PlayerPrefs.GetFloat("SoundEffectsVolume", 0.5f);
        initialMusicVolume = musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        initialHapticsIntensity = hapticsSlider.value = PlayerPrefs.GetFloat("HapticsIntensity", 0.5f);
    }

    void SaveSettings()
    {
        PlayerPrefs.SetFloat("SoundEffectsVolume", soundEffectsVolumeSlider.value);
        PlayerPrefs.SetFloat("MusicVolume", musicVolumeSlider.value);
        PlayerPrefs.SetFloat("HapticsIntensity", hapticsSlider.value);
        PlayerPrefs.Save();

        HandleSoundEffectsVolumeChanged(soundEffectsVolumeSlider.value);
        HandleMusicVolumeChanged(musicVolumeSlider.value);

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

    void HandleMusicVolumeChanged(float volume)
    {
        if (AudioManager != null)
        {
            AudioManager.AdjustMusicVolume(volume);
        }
    }

    void HandleSoundEffectsVolumeChanged(float sliderValue)
    {
        float mappedVolume = sliderValue * 0.1f;
        if (AudioManager != null)
        {
            AudioManager.AdjustAmbientVolume(mappedVolume);
        }
    }
}
