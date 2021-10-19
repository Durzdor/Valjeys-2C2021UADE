using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class CharacterSettings : MonoBehaviour
{
    #region SerializedFields

#pragma warning disable 649
    [Header("References")] [Space(2)] 
    [SerializeField] private Toggle invertMouseYToggle;
    [SerializeField] private Toggle invertMouseXToggle;
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider effectsVolumeSlider;
    [SerializeField] private AudioMixer audioMixer;
#pragma warning restore 649

    #endregion

    private const string MIXER_MASTER_VOLUME = "MasterVolume";
    private const string MIXER_MUSIC_VOLUME = "MusicVolume";
    private const string MIXER_EFFECTS_VOLUME = "EffectsVolume";
    public bool InvertMouseY { get; private set; }
    public bool InvertMouseX { get; private set; }

    private void Start()
    {
        invertMouseYToggle.onValueChanged.AddListener(delegate { InvertYToggle(invertMouseYToggle); });
        invertMouseXToggle.onValueChanged.AddListener(delegate { InvertXToggle(invertMouseXToggle); });
        musicVolumeSlider.onValueChanged.AddListener(delegate { ChangeBGMVolume(musicVolumeSlider); });
        masterVolumeSlider.onValueChanged.AddListener(delegate { ChangeMasterVolume(masterVolumeSlider); });
        effectsVolumeSlider.onValueChanged.AddListener(delegate { ChangeEffectsVolume(effectsVolumeSlider); });
        ReadInitialValues();
    }

    private void ReadInitialValues()
    {
        InvertYToggle(invertMouseYToggle);
        InvertXToggle(invertMouseXToggle);
        ChangeBGMVolume(musicVolumeSlider);
    }

    private void InvertYToggle(Toggle check)
    {
        InvertMouseY = check.isOn;
    }

    private void InvertXToggle(Toggle check)
    {
        InvertMouseX = check.isOn;
    }

    private void ChangeMasterVolume(Slider slider)
    {
        audioMixer.SetFloat(MIXER_MASTER_VOLUME, Mathf.Log(slider.value) * 20);
    }
    
    private void ChangeBGMVolume(Slider slider)
    {
        audioMixer.SetFloat(MIXER_MUSIC_VOLUME, Mathf.Log(slider.value) * 20);
    }
    
    private void ChangeEffectsVolume(Slider slider)
    {
        audioMixer.SetFloat(MIXER_EFFECTS_VOLUME, Mathf.Log(slider.value) * 20);
    }
}
