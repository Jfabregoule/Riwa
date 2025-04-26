using UnityEngine;
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
    public void SetSoundFXVolume(float level) => RiwaSoundMixerManager.Instance.SetSoundFXVolume(level);
    public void SetMasterVolume(float level) => RiwaSoundMixerManager.Instance.SetMasterVolume(level);
    public void SetCinematicVolume(float level) => RiwaSoundMixerManager.Instance.SetCinematicVolume(level);
    public void SetVibration(bool enabled) => VibrationSystem.Instance.SetVibrationEnabled(enabled);
    public void SetAllVolume()
    {
        //RiwaSoundMixerManager.Instance.SetMasterVolume(level);
        //RiwaSoundMixerManager.Instance.SetMusicVolume(level);
        //RiwaSoundMixerManager.Instance.SetSoundFXVolume(level);
        //RiwaSoundMixerManager.Instance.SetCinematicVolume(level);
    }



}
