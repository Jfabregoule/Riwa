using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private float _holdTime = 2.0f;

    public delegate void HoldCompleteEvent();
    public HoldCompleteEvent OnHoldComplete;

    private Coroutine _currentCoroutine;
    private Image _fillImage;
    private float _timer = 0f;
   
    void Start()
    {
        _fillImage = transform.Find("Fill").GetComponent<Image>();
    }

    
    IEnumerator PressedButton()
    {
        _timer = _fillImage.fillAmount * _holdTime;

        while (_timer < _holdTime)
        {
            _timer += Time.deltaTime;
            _fillImage.fillAmount = _timer / _holdTime;
            yield return null;
        }

        _fillImage.fillAmount = 1f;
        HoldComplete();
    }

    IEnumerator ReleaseButton()
    {
        _timer = _fillImage.fillAmount * _holdTime;

        while (_timer > 0)
        {
            _timer -= Time.deltaTime;
            _fillImage.fillAmount = _timer / _holdTime;
            yield return null;
        }

        _fillImage.fillAmount = 0f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_currentCoroutine != null) StopCoroutine(_currentCoroutine);
        _currentCoroutine = StartCoroutine(ReleaseButton());    
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_currentCoroutine != null) StopCoroutine(_currentCoroutine);
        _currentCoroutine = StartCoroutine(PressedButton());
    }

    private void HoldComplete()
    {
        OnHoldComplete?.Invoke();
    }
}
