using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecepterLaser : MonoBehaviour
{
    private bool _isActive;
    private bool _isHitThisFrame;

    [SerializeField] private GameObject[] _activables;

    void Update()
    {
        if (!_isHitThisFrame && _isActive)
        {
            Deactivate();
        }

        _isHitThisFrame = false;
    }

    public void OnLaserHit()
    {
        if (!_isActive)
        {
            Activate();
        }

        _isHitThisFrame = true;
    }

    private void Activate()
    {
        _isActive = true;
        foreach (var activable in _activables)
        {
            if (activable.TryGetComponent(out IActivable act))
            {
                act.Activate();
            }  
        }
        Debug.Log("Recepteur activé !");
    }

    private void Deactivate()
    {
        _isActive = false;
        foreach (var activable in _activables)
        {
            if (activable.TryGetComponent(out IActivable act))
            {
                act.Deactivate();
            }
        }
        Debug.Log("Recepteur désactivé !");
    }
}
