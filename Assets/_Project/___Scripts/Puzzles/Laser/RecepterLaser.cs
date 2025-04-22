using System.Collections;
using UnityEngine;

public class RecepterLaser : MonoBehaviour, IActivable
{
    private bool _isActive;
    private bool _isHitThisFrame;

    private Coroutine _activationCoroutine;

    public event IActivable.ActivateEvent OnActivated;
    public event IActivable.ActivateEvent OnDesactivated;

    void LateUpdate()
    {
        if (!_isHitThisFrame && _isActive)
        {
            Deactivate();
        }

        _isHitThisFrame = false;
    }

    public void OnLaserHit()
    {
        _isHitThisFrame = true;

        if (!_isActive && _activationCoroutine == null)
        {
            _activationCoroutine = StartCoroutine(DelayedActivation());
        }
    }

    public void Activate()
    {
        _isActive = true;
        OnActivated?.Invoke();
        Debug.Log("Recepteur activé !");
    }

    public void Deactivate()
    {
        _isActive = false;
        OnDesactivated?.Invoke();
        Debug.Log("Recepteur désactivé !");
    }

    private IEnumerator DelayedActivation()
    {
        float duration = 0.1f;
        yield return Helpers.GetWait(duration);

        if (_isHitThisFrame)
        {
            Activate();
        }
        _activationCoroutine = null;
    }
}
