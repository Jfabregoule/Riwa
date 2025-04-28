using System.Collections;
using UnityEngine;

public class RecepterLaser : MonoBehaviour, IActivable
{
    private bool _isActive;
    private bool _isHitThisFrame;
    private Renderer[] _renderers;

    private Coroutine _activationCoroutine;

    public event IActivable.ActivateEvent OnActivated;
    public event IActivable.ActivateEvent OnDesactivated;

    void Start()
    {
        _renderers = transform.GetChild(0).GetChild(0).GetComponentsInChildren<Renderer>();
    }
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
        foreach (Renderer renderer in _renderers)
        {
            Material material = renderer.material;
            if (material.HasProperty("_IsActivated"))
                material.SetFloat("_IsActivated", 1f);
        }
        OnActivated?.Invoke();
    }

    public void Deactivate()
    {
        _isActive = false;
        foreach (Renderer renderer in _renderers)
        {
            Material material = renderer.material;
            if (material.HasProperty("_IsActivated"))
                material.SetFloat("_IsActivated", 0f);
        }
        OnDesactivated?.Invoke();
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
