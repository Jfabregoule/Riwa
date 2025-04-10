using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;

public class Preference : MonoBehaviour
{
    [SerializeField] BinaryChoice _audioBinary;
    [SerializeField] BinaryChoice _vibrationBinary;
    [SerializeField] BinaryChoice _controlBinary;

    private SaveData _saveData;
    struct SaveData
    {
        public bool IsAudioEnable;
        public bool IsVibrationEnable;
        public bool IsControlEnable;
    }

    private void OnEnable()
    {
        _audioBinary.OnValueChange += SetAudioBinary;
        _vibrationBinary.OnValueChange += SetVibrationBinary;
        _controlBinary.OnValueChange += SetControlBinary;
    }

    private void Start()
    {
        _saveData = new SaveData();
        _saveData.IsAudioEnable = true;
        _saveData.IsVibrationEnable = true;
        _saveData.IsControlEnable = true;
    }

    
    private void SetAudioBinary(bool value)
    {
        _saveData.IsAudioEnable = value;
    }

    private void SetVibrationBinary(bool value)
    {
        _saveData.IsVibrationEnable= value;
    }

    private void SetControlBinary(bool value)
    {
        _saveData.IsControlEnable = value;
    }

}
