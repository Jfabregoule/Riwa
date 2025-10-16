using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderComponent : MonoBehaviour
{
    [SerializeField] private string _name;
    [SerializeField] private TextMeshProUGUI _valueText;

    private Slider _slider;
    private UiSoundPlayer _soundPlayer;
    private void OnEnable()
    {
        SaveSystem.Instance.OnLoadSettings += LoadingSlider;
        SaveSystem.Instance.OnSaveSettings += SaveSlider;
    }

    private void Start()
    {
        _slider = GetComponent<Slider>();
        _slider.value = SaveSystem.Instance.LoadElement<float>(_name, true);
        _soundPlayer = GetComponent<UiSoundPlayer>();

        _slider.onValueChanged.AddListener(OnVolumeChanged);
        UpdateText(_slider.value);
    }

    private void LoadingSlider()
    {
        _slider.value = SaveSystem.Instance.LoadElement<float>(_name, true);
    }

    private void SaveSlider()
    {
        SaveSystem.Instance.SaveElement(_name, _slider.value, true);
    }

    public void UpdateText(float value)
    {
        _valueText.text = value.ToString();
    }

    private void OnVolumeChanged(float value) 
    {
        _soundPlayer.PlaySound();
    }
}
