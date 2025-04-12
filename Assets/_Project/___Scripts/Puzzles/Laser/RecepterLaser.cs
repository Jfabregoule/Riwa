using UnityEngine;

public class RecepterLaser : MonoBehaviour, IActivable
{
    private bool _isActive;
    private bool _isHitThisFrame;

    public event IActivable.ActivateEvent OnActivated;
    public event IActivable.ActivateEvent OnDesactivated;

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

    public void Activate()
    {
        _isActive = true;
        OnActivated?.Invoke();
        Debug.Log("Recepteur activ� !");
    }

    public void Deactivate()
    {
        _isActive = false;
        OnDesactivated?.Invoke();
        Debug.Log("Recepteur d�sactiv� !");
    }
}
