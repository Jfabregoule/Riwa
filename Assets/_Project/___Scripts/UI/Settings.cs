using UnityEngine;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;

public class Settings : MonoBehaviour
{

    [Header("Language")]
    [SerializeField] private LanguageSelector _languageSelector;

    private void OnEnable()
    {

        //_languageSelector.OnChangeLanguage += SetLanguage;
    }
    private void OnDisable()
    {
        //_languageSelector.OnChangeLanguage -= SetLanguage;
    }


    //private void SetLanguage(int language) => _saveData.CurrentLanguage = language;
    public void SetMusicVolume(float level) => RiwaSoundMixerManager.Instance.SetMusicVolume(level);
    public void SetSoundFXVolume(float level) {
        RiwaSoundMixerManager.Instance.SetSoundFXVolume(level);
        RiwaSoundMixerManager.Instance.SetAmbiantVolume(level);
    }
    public void SetMasterVolume(float level) => RiwaSoundMixerManager.Instance.SetMasterVolume(level);
    public void SetCinematicVolume(float level) => RiwaSoundMixerManager.Instance.SetCinematicVolume(level);
    public void SetVibration(bool enabled) => VibrationSystem.Instance.SetVibrationEnabled(enabled);
    public void SetGlobalVolume(Slider slider)
    {
        float value = SaveSystem.Instance.LoadElement<float>("_masterTempValue", true);
        RiwaSoundMixerManager.Instance.SetMasterVolume(value);
        slider.value = value;
        SliderComponent sliderText = slider.GetComponent<SliderComponent>();
        sliderText.UpdateText(value);
    }
    public void SaveTempMasterValue(Slider slider)
    {
        SaveSystem.Instance.SaveElement<float>("_masterTempValue", slider.value, true);
    }
    public void SaveSettings()
    {
        SaveSystem.Instance.SaveSettingsData();
    }

}
