using TMPro;
using UnityEngine;

public class Control : MonoBehaviour
{
    [SerializeField] private BinaryChoice _binaryChoice;

    [SerializeField] private TextMeshProUGUI _textLeft;
    [SerializeField] private TranslateText _translateTextLeft;
    [SerializeField] private TranslateText _translateTextRight;
    [SerializeField] private TextMeshProUGUI _textRight;
    [SerializeField] private TranslateText _translateTextMode;
    [SerializeField] private SentenceTranslate _rightModeSentence;
    [SerializeField] private SentenceTranslate _leftModeSentence;

    private SentenceTranslate _interactText;
    private SentenceTranslate _joystickText;

    private bool _isRightHanded;

    void Start()
    {
        _isRightHanded = true;
        _interactText = _translateTextLeft.GetSentenceTranslate();
        _joystickText = _translateTextRight.GetSentenceTranslate();
        UpdateControl();
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
        _translateTextLeft.SetSentenceTranslate(_interactText);
        _translateTextRight.SetSentenceTranslate(_joystickText);
        _translateTextMode.SetSentenceTranslate(_rightModeSentence);
    }

    private void LeftHanded()
    {
        _translateTextLeft.SetSentenceTranslate(_joystickText);
        _translateTextRight.SetSentenceTranslate(_interactText);
        _translateTextMode.SetSentenceTranslate(_leftModeSentence);

    }

    private void SetHanded(bool isRightHanded) 
    { 
        _isRightHanded = isRightHanded;
        UpdateBinaryChoice();
    }
}
