using System.Collections;
using UnityEngine;

public class PressurePlate : MonoBehaviour, IActivable
{
    public event IActivable.ActivateEvent OnActivated;
    public event IActivable.ActivateEvent OnDesactivated;

    [SerializeField] private float _lerpTime = 0.3f;
    [SerializeField] private Vector3 _offset;
    private Vector3 _initialPosition;
    private Vector3 _destination;

    private void OnTriggerEnter(Collider other)
    {
        Activate();
    }

    private void OnTriggerExit(Collider other)
    {
        Deactivate();
    }

    public void Activate()
    {
        _initialPosition = transform.position;
        _destination = _initialPosition + _offset;
        StartCoroutine(LerpPressed(true));
    }

    public void Deactivate()
    {
        _initialPosition = transform.position;
        _destination = _initialPosition - _offset;
        StartCoroutine(LerpPressed(false));   
    }

    IEnumerator LerpPressed(bool active)
    {
        float elapsedTime = 0.0f;
        while (elapsedTime < _lerpTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / _lerpTime;
            t = Mathf.Clamp01(t);
            transform.position = Vector3.Lerp(_initialPosition, _destination, t);
            yield return null;
        }
        transform.position = _destination;
        if (active)
        {
            OnActivated?.Invoke();
        }
        else
        {
            OnDesactivated?.Invoke();
        }
    }
}
