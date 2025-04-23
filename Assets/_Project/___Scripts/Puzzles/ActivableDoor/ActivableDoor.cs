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

    [SerializeField] private List<CinemachineVirtualCamera> _doorCameras; // NEED REF CURRENT LEVEL MANAGER

    private List<IActivable> _activables = new List<IActivable>();
    private List<IActivable> _activatedItem = new List<IActivable>();
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

        Debug.Log("Activable list count: " + _activables.Count);
    }

    private void OnEnable()
    {
        foreach (IActivable activable in _activables)
        {
            Debug.Log("Activated gameobject list: " +  activable);
            activable.OnActivated += () => HandleActivatedActivable(activable, true);
            activable.OnDesactivated += () => HandleActivatedActivable(activable, false);
        }
    }

    private void OnDisable()
    {
        foreach (IActivable activable in _activables)
        {
            activable.OnActivated -= () => HandleActivatedActivable(activable, true);
            activable.OnDesactivated -= () => HandleActivatedActivable(activable, false);
        }
    }

    private void HandleActivatedActivable(IActivable activatedOne, bool save)
    {
        if(save)
            _activatedItem.Add(activatedOne);
        else
            _activatedItem.Remove(activatedOne);

        if (_activatedItem.Count == _activables.Count) 
            OpenDoor();
    }

    private void Update()
    {
        Debug.Log("activatedItemCount: " + _activatedItem.Count);
        for(int i = 0; i < _activatedItem.Count; i++)
        {
            Debug.Log(_activatedItem[i]);
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

        if (_doorCameras.Count > 0)
        {
            _doorCameras[0].Priority = 20;
            yield return new WaitForSeconds(2f);
            _doorCameras[1].Priority = 25;
            yield return new WaitForSeconds(2f);
        }

        Vector3 start = transform.position;
        float elapsed = 0f;

        while (elapsed < _lerpTime)
        {
            transform.position = Vector3.Lerp(start, targetPosition, elapsed / _lerpTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;

        if (_doorCameras.Count > 0)
        {
            _doorCameras[1].Priority = 0;
            yield return new WaitForSeconds(2f);
            _doorCameras[0].Priority = 0;
            yield return new WaitForSeconds(2f);
        }
    }
}
