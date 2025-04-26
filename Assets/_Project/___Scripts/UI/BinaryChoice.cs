using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BinaryChoice : MonoBehaviour
{
    [Header("Button Yes / No")]
    [SerializeField] private string _binaryName;
    [SerializeField] private Button _yesButton;
    [SerializeField] private Button _noButton;

    [Header("Textes")]
    private TextMeshProUGUI _yesText;
    private TextMeshProUGUI _noText;
    
    [Header("Color")]
    [SerializeField] private Color _selectedColor;
    [SerializeField] private Color _defaultColor;

    public delegate void ValueChangeEvent(bool value);
    public ValueChangeEvent OnValueChange;

    public string BinaryName { get { return _binaryName; } set {_binaryName = value; } }
    public bool Value {  get; private set; }

    private void OnEnable()
    {
        OnValueChange += SetHanded;
    }
    private void Awake()
    {
        _yesText = _yesButton.GetComponent<TextMeshProUGUI>();
        _noText = _noButton.GetComponent<TextMeshProUGUI>();
        SaveSystem.Instance.OnLoadSettings += LoadBinary;
        SaveSystem.Instance.OnSaveSettings += SaveBinary;

    }

    void Start()
    {
        _yesButton.onClick.AddListener(() => SetValue(true));
        _noButton.onClick.AddListener(() => SetValue(false));

    }
    public void SetValue(bool isEnable)
    {
        InvokeEvent(isEnable);
    }

    private void SetHanded(bool isRightHanded)
    {
        Value = isRightHanded;
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        _yesText.color = Value ? _selectedColor : _defaultColor;
        _noText.color = Value ? _defaultColor : _selectedColor;
    }

    public void InvokeEvent(bool IsEnable)
    {
        OnValueChange?.Invoke(IsEnable);
    }

    void LoadBinary()
    {
        Value = SaveSystem.Instance.LoadElement<bool>(_binaryName, true);
        UpdateVisuals();
    }

    public void SaveBinary()
    {
        SaveSystem.Instance.SaveElement(_binaryName, Value, true);
    }
}
