using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeTimeColor : MonoBehaviour
{
    [SerializeField] private Color _pastColor;
    [SerializeField] private Color _presentColor;

    private Image _image;
    private bool _isPresent;

    private void OnEnable()
    {
        GameManager.Instance.OnTimeChangeStarted += SetColor;
    }
    void Start()
    {
        _isPresent = true;
        _image = GetComponent<Image>();
    }

    private void SetColor(EnumTemporality temporality) {
        _isPresent = !_isPresent;
        if (_isPresent)
            _image.color = _presentColor;
        else
            _image.color = _pastColor;
    }
}
