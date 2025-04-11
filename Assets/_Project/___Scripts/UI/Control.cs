using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Control : MonoBehaviour
{
    [SerializeField] private BinaryChoice _binaryChoice;

    [SerializeField] private TextMeshProUGUI _textLeft;
    [SerializeField] private TextMeshProUGUI _textRight;
    [SerializeField] private TextMeshProUGUI _textMode;

    private string _interactText;
    private string _joystickText;

    private bool _isRightHanded;

    void Start()
    {
        _isRightHanded = true;
        _interactText = _textLeft.text;
        _joystickText = _textRight.text;
        //UpdateControl();
    }
    private void OnEnable()
    {
        _binaryChoice.OnValueChange += SetHanded;
    }

    private void OnDisable()
    {
        _binaryChoice.OnValueChange -= SetHanded;
    }

    public void UpdateControl()
    {
        _binaryChoice.InvokeEvent(!_isRightHanded);
    }
    private void UpdateBinaryChoice()
    {
        if (_isRightHanded)
            RightHanded();
        else
            LeftHanded();
    }

    private void RightHanded()
    {
        _textRight.text = _joystickText;
        _textLeft.text = _interactText;
        _textMode.text = "mode droitier";
    }

    private void LeftHanded()
    {
        _textRight.text = _interactText;
        _textLeft.text = _joystickText;
        _textMode.text = "mode gaucher";
    }

    private void SetHanded(bool isRightHanded) 
    { 
        _isRightHanded = isRightHanded;
        UpdateBinaryChoice();
    }
}
