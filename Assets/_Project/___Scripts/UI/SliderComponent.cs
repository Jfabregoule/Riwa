using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderComponent : MonoBehaviour
{
    [SerializeField] private string _name;
    [SerializeField] private TextMeshProUGUI _valueText;

    private Slider _slider;
    private UiSoundPlayer _soundPlayer;
    private bool _isInitializing;
    private void OnEnable()
    {
        SaveSystem.Instance.OnLoadSettings += LoadingSlider;
        SaveSystem.Instance.OnSaveSettings += SaveSlider;
    }
    void Start()
    {
        _isInitializing = true;
        _slider = GetComponent<Slider>();
        _slider.value = SaveSystem.Instance.LoadElement<float>(_name, true);
        _soundPlayer = GetComponent<UiSoundPlayer>();
        _isInitializing = false;

        _slider.onValueChanged.AddListener(OnVolumeChanged);
        UpdateText(_slider.value);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadingSlider()
    {
        _slider.value = SaveSystem.Instance.LoadElement<float>(_name, true);
    }

    public void SaveSlider()
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
