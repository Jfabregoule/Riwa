using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Control : MonoBehaviour
{
    [SerializeField] private BinaryChoice _binaryChoice;
    [SerializeField] private CanvasGroup _controlSettings;

    [SerializeField] private CanvasGroup _interactLeftPart;
    [SerializeField] private CanvasGroup _joystickLeftPart;
    [SerializeField] private CanvasGroup _interactRightPart;
    [SerializeField] private CanvasGroup _joystickRightPart;
    [SerializeField] private TranslateText _translateTextMode;
    [SerializeField] private SentenceTranslate _rightModeSentence;
    [SerializeField] private SentenceTranslate _leftModeSentence;

    [SerializeField] private List<CanvasGroup> _uiLeft;
    [SerializeField] private List<CanvasGroup> _uiRight;


    private CanvasGroup _canvasGroup;
    private bool _isRightHanded;

    private bool _isFirstControl;

    public bool IsRightHanded { get => _isRightHanded; set => _isRightHanded = value; }

    void Start()
    {
        SaveSystem.Instance.OnLoadSettings += LoadingHanded;
        //_isRightHanded = SaveSystem.Instance.LoadElement<bool>("_isRightHanded", true);
        _canvasGroup = GetComponent<CanvasGroup>();
        _isFirstControl = SaveSystem.Instance.LoadElement<bool>("IsFirstControl", false);
        //UpdateBinaryChoice();
        //InvertControlUI();
    }
    private void OnEnable()
    {
        _binaryChoice.OnValueChange += SetHanded;
        GameManager.Instance.OnShowBasicInputEvent += ShowInput;
    }

    private void OnDisable()
    {
        _binaryChoice.OnValueChange -= SetHanded;

    }

    public void UpdateControl()
    {
        _binaryChoice.InvokeEvent(!_isRightHanded);
        ToggleControlInvert(!_isRightHanded);
       
    }
    private void UpdateBinaryChoice()
    {
        if (_isRightHanded)
            RightHanded();
        else
            LeftHanded();

        ToggleControlInvert(!_isRightHanded);
        InvertControlUI();
    }

    private void RightHanded()
    {
        Helpers.EnabledCanvasGroup(_interactRightPart);
        Helpers.EnabledCanvasGroup(_joystickLeftPart);
        Helpers.DisabledCanvasGroup(_interactLeftPart);
        Helpers.DisabledCanvasGroup(_joystickRightPart);
        _translateTextMode.SetSentenceTranslate(_rightModeSentence);
    }

    private void LeftHanded()
    {
        Helpers.DisabledCanvasGroup(_interactRightPart);
        Helpers.DisabledCanvasGroup(_joystickLeftPart);
        Helpers.EnabledCanvasGroup(_interactLeftPart);
        Helpers.EnabledCanvasGroup(_joystickRightPart);
        _translateTextMode.SetSentenceTranslate(_leftModeSentence);
    }

    private void SetHanded(bool isRightHanded) 
    { 
        _isRightHanded = isRightHanded;
        UpdateBinaryChoice();
    }

    public void ToggleControlInvert(bool isInvert)
    {
        InputManager.Instance.ToggleControlInversion(isInvert);
    }

    public void InvertControlUI()
    {
        if (_isRightHanded)
            UIRight();
        else
            UILeft();
    }

    private void UILeft()
    {
        for(int i = 0; i < _uiRight.Count; i++)
        {
            Helpers.DisabledCanvasGroup(_uiRight[i]);
        }
        for (int i = 0; i<_uiLeft.Count;i++)
        {
            if (i == 0 && !GameManager.Instance.ChangeTimeUnlock) continue;

            Helpers.EnabledCanvasGroup(_uiLeft[i]);
        }
        
    }
    private void UIRight()
    {
        for (int i = 0; i < _uiLeft.Count; i++)
        {
            Helpers.DisabledCanvasGroup(_uiLeft[i]);
        }
        for (int i = 0; i < _uiRight.Count; i++)
        {
            if (i == 0 && !GameManager.Instance.ChangeTimeUnlock) continue;

            Helpers.EnabledCanvasGroup(_uiRight[i]);
        }
    }

    public void ShowInput()
    {
        if (!_isFirstControl) return;
        Helpers.EnabledCanvasGroup(_canvasGroup);
        Helpers.EnabledCanvasGroup(_controlSettings);
        _isFirstControl = false;
        SaveSystem.Instance.SaveElement<bool>("IsFirstControl", _isFirstControl, false);
        GameManager.Instance.OnShowBasicInputEvent -= ShowInput;
        StartCoroutine(WaitSeconds(5f));
    }

    private IEnumerator WaitSeconds(float duration)
    {
        yield return new WaitForSeconds(duration);
        Helpers.DisabledCanvasGroup(_canvasGroup);
        Helpers.DisabledCanvasGroup(_controlSettings);
    }

    public void LoadingHanded()
    {
        _isRightHanded = SaveSystem.Instance.LoadElement<bool>("_isRightHanded", true);
        UpdateBinaryChoice();
        InvertControlUI();
    }

}
