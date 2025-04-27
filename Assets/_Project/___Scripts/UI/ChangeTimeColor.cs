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
    private Coroutine _colorCoroutine;

    private void OnEnable()
    {
        GameManager.Instance.OnTimeChangeStarted += SetColor;
    }
    void Start()
    {
        _isPresent = true;
        _image = GetComponent<Image>();
    }

    private void SetColor(EnumTemporality temporality)
    {
        _isPresent = !_isPresent;

        if (_colorCoroutine != null)
            StopCoroutine(_colorCoroutine);

        _colorCoroutine = StartCoroutine(LerpColor(_isPresent ? _pastColor : _presentColor, _isPresent ? _presentColor : _pastColor, 1f));
    }

    private IEnumerator LerpColor(Color fromColor, Color toColor, float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / duration);
            _image.color = Color.Lerp(fromColor, toColor, t);
            yield return null;
        }
        _image.color = toColor;
    }
}
