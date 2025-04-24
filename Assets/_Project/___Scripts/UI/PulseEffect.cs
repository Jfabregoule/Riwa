using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PulseEffect : MonoBehaviour
{
    [SerializeField] private float _factor = 1.1f;
    [SerializeField] private float _duration = 0.5f ;

    [SerializeField] private bool _isPulsing;
    private Vector3 _initialScale;

    private Coroutine _pulseCoroutine;
    void Start()
    {
        _initialScale = transform.localScale;
        if (!_isPulsing) return;
        StartPulsing();
    }

    IEnumerator Pulse()
    {
        while (_isPulsing)
        {
            yield return ScaleTo(_initialScale * _factor, _duration / 2f);

            yield return ScaleTo(_initialScale, _duration / 2f);
        }
    }

    IEnumerator ScaleTo(Vector3 targetScale, float duration)
    {
        Vector3 startScale = transform.localScale;
        float time = 0f;

        while (time < duration)
        {
            transform.localScale = Vector3.Lerp(startScale, targetScale, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;
    }

    public void StopPulsing(EnumTemporality temporality)
    {
        _isPulsing = false;

        if(_pulseCoroutine != null)
        {
            StopCoroutine(_pulseCoroutine);
            _pulseCoroutine = null;
            transform.localScale = new Vector3(1f,1f, 1f);
        }
    }

    public void StartPulsing()
    {
        _isPulsing = true;
        _pulseCoroutine = StartCoroutine(Pulse());
    }
}
