using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivableDoor : MonoBehaviour
{
    [SerializeField] MonoBehaviour[] _activableComponents;
    [SerializeField] Vector3 _openingOffset;
    [SerializeField] float _lerpTime = 1.0f;

    private List<IActivable> _activables = new List<IActivable>();
    private Vector3 _closedPosition;
    private Vector3 _openedPosition;
    private Coroutine _currentLerp;

    private void Awake()
    {
        _closedPosition = transform.position;
        _openedPosition = _closedPosition + _openingOffset;

        foreach (MonoBehaviour monoBehaviour in _activableComponents)
        {
            if (monoBehaviour is IActivable activable)
                _activables.Add(activable);
        }
    }

    private void OnEnable()
    {
        foreach (IActivable activable in _activables)
        {
            activable.OnActivated += OpenDoor;
            activable.OnDesactivated += CloseDoor;
        }
    }

    private void OnDisable()
    {
        foreach (IActivable activable in _activables)
        {
            activable.OnActivated -= OpenDoor;
            activable.OnDesactivated -= CloseDoor;
        }
    }

    private void OpenDoor()
    {
        if (_currentLerp != null) StopCoroutine(_currentLerp);
        _currentLerp = StartCoroutine(LerpDoorPosition(_openedPosition));
    }

    private void CloseDoor()
    {
        if (_currentLerp != null) StopCoroutine(_currentLerp);
        _currentLerp = StartCoroutine(LerpDoorPosition(_closedPosition));
    }

    private IEnumerator LerpDoorPosition(Vector3 targetPosition)
    {
        Vector3 start = transform.position;
        float elapsed = 0f;

        while (elapsed < _lerpTime)
        {
            transform.position = Vector3.Lerp(start, targetPosition, elapsed / _lerpTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
    }
}
