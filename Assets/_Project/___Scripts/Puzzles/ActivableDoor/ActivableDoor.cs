using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Newtonsoft.Json.Bson;
using UnityEngine;

public class ActivableDoor : MonoBehaviour
{
    [SerializeField] MonoBehaviour[] _activableComponents;
    [SerializeField] Vector3 _openingOffset;
    [SerializeField] float _lerpTime = 1.0f;

    [SerializeField] private List<CinemachineVirtualCamera> _doorCameras;

    private int _currentActivated = 0;
    private Vector3 _closedPosition;
    private Coroutine _currentLerp;

    private void Start()
    {
        foreach(var activable in _activableComponents)
        {
            if(activable.TryGetComponent(out IActivable act))
            {
                act.OnActivated += OnActivableActivated;
                act.OnDesactivated += OnActivableDeactivated;
            }
        }
    }

    private void OnDisable()
    {
        foreach (IActivable activable in _activableComponents)
        {
            activable.OnActivated -= OnActivableActivated;
            activable.OnDesactivated -= OnActivableDeactivated;
        }
    }

    private void CheckDoorState()
    {
        if(_currentActivated == _activableComponents.Length)
            OpenDoor();
        //else
        //    CloseDoor();
    }

    private void OnActivableActivated()
    {
        _currentActivated += 1;
        CheckDoorState();
    }

    private void OnActivableDeactivated()
    {
        _currentActivated -= 1;
    }

    private void OpenDoor()
    {
        if (_currentLerp != null) StopCoroutine(_currentLerp);
        _currentLerp = StartCoroutine(LerpDoorPosition(_openingOffset));
    }

    private void CloseDoor()
    {
        if (_currentLerp != null) StopCoroutine(_currentLerp);
        _currentLerp = StartCoroutine(LerpDoorPosition(_closedPosition));
    }

    private IEnumerator LerpDoorPosition(Vector3 targetPosition)
    {

        GameManager.Instance.Character.InputManager.DisableGameplayControls();
        if (_doorCameras.Count > 0)
        {
            _doorCameras[0].Priority = 20;
            yield return new WaitForSeconds(1.5f);
            _doorCameras[1].Priority = 25;
            yield return new WaitForSeconds(1.5f);
        }

        Vector3 start = transform.position;

        if (targetPosition.x == 0f) targetPosition.x = start.x;
        if (targetPosition.y == 0f) targetPosition.y = start.y;
        if (targetPosition.z == 0f) targetPosition.z = start.z;

        float elapsed = 0f;

        while (elapsed < _lerpTime)
        {
            elapsed += Time.deltaTime;
            transform.position = Vector3.Lerp(start, targetPosition, elapsed / _lerpTime);
            yield return null;
        }

        transform.position = targetPosition;

        if (_doorCameras.Count > 0)
        {
            _doorCameras[1].Priority = 0;
            yield return new WaitForSeconds(1.5f);
            _doorCameras[0].Priority = 0;
            yield return new WaitForSeconds(1.5f);
        }
        GameManager.Instance.Character.InputManager.EnableGameplayControls();
    }
}
