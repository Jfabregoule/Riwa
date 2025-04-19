using UnityEngine;

public class Settings : MonoBehaviour
{
    [Header("Preference Part")]
    [SerializeField] private BinaryChoice _audioBinary;
    [SerializeField] private BinaryChoice _vibrationBinary;
    [SerializeField] private BinaryChoice _controlBinary;

    [Header("Language")]
    [SerializeField] private LanguageSelector _languageSelector;

    [Header("Control")]
    [SerializeField] private Control _control;

    private SaveData _saveData;
    struct SaveData
    {
        public bool IsAudioEnable;
        public bool IsVibrationEnable;
        public bool IsRightHanded;
        public int CurrentLanguage;
    }

    private void OnEnable()
    {
        _audioBinary.OnValueChange += SetAudioBinary;
        _vibrationBinary.OnValueChange += SetVibrationBinary;
        _controlBinary.OnValueChange += SetControlPos;
        //_languageSelector.OnChangeLanguage += SetLanguage;
    }
    private void OnDisable()
    {
        _audioBinary.OnValueChange -= SetAudioBinary;
        _vibrationBinary.OnValueChange -= SetVibrationBinary;
        _controlBinary.OnValueChange -= SetControlPos;
        //_languageSelector.OnChangeLanguage -= SetLanguage;
    }
    private void Start()
    {
        _saveData = new SaveData();
        _saveData.IsAudioEnable = true;
        _saveData.IsVibrationEnable = true;
        _saveData.IsRightHanded = true;
    }

    private void SetAudioBinary(bool value)
    {
        _saveData.IsAudioEnable = value;
    }

    private void SetVibrationBinary(bool value)
    {
        _saveData.IsVibrationEnable = value;
    }

    private void SetControlPos(bool value)
    {
        _saveData.IsRightHanded = value;
        Debug.Log(value);
    }

    private void SetLanguage(int language)
    {
        _saveData.CurrentLanguage = language;
    }

    public void SetMusicVolume(float level)
    {
        RiwaSoundMixerManager.Instance.SetMusicVolume(level);
    }

    public void SetSoundFXVolume(float level)
    {
        RiwaSoundMixerManager.Instance.SetSoundFXVolume(level);
    }
}
