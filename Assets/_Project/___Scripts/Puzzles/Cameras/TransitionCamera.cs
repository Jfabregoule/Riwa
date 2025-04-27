using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionCamera : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera[] _cameras;
    [SerializeField] private MonoBehaviour[] _activables;
    [SerializeField] private float _waitOnCamera = 3f;
    private int CurrentActive;
    void Start()
    {
        foreach (var activable in _activables)
        {
            if (activable.TryGetComponent(out IActivable act))
            {
                act.OnActivated += AddActivate;
                act.OnDesactivated += RemoveActivate;
            }
        }
    }

    private void AddActivate()
    {
        CurrentActive++;
        if (CurrentActive == _activables.Length)
        {
            GameManager.Instance.Character.StateMachine.GoToIdle();
            StartCoroutine(SwitchCamera());
        }
    }

    private void RemoveActivate()
    {
        if (CurrentActive != _activables.Length)
        {
            CurrentActive--;
        }
    }

    private IEnumerator SwitchCamera()
    {
        InputManager.Instance.DisableGameplayControls();
        _cameras[0].Priority = 20;
        yield return new WaitForSeconds(_waitOnCamera);
        for (int i = 0; i < _cameras.Length - 1; i++)
        {
            _cameras[i].Priority = 0;
            _cameras[i + 1].Priority = 20;
            yield return new WaitForSeconds(_waitOnCamera);
        }
        _cameras[_cameras.Length - 1].Priority = 0;
        InputManager.Instance.EnableGameplayControls();
    }
}
