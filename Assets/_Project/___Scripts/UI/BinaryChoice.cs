using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BinaryChoice : MonoBehaviour
{
    [Header("Button Yes / No")]
    [SerializeField] private Button _yesButton;
    [SerializeField] private Button _noButton;

    [Header("Textes")]
    private TextMeshProUGUI _yesText;
    private TextMeshProUGUI _noText;
    
    [Header("Color")]
    [SerializeField] private Color _selectedColor;
    [SerializeField] private Color _defaultColor;

    public delegate void ValueChange(bool value);
    public ValueChange OnValueChange;

    public bool Value {  get; private set; }

    void Start()
    {
        Value = true;
        _yesButton.onClick.AddListener(() => SetValue(true));
        _noButton.onClick.AddListener(() => SetValue(false));
        _yesText = _yesButton.GetComponent<TextMeshProUGUI>();
        _noText = _noButton.GetComponent<TextMeshProUGUI>();

        UpdateVisuals();
    }

    public void SetValue(bool isEnable)
    {
        Value = isEnable;
        InvokeEvent(Value);
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
}
