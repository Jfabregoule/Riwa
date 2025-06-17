using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBoxActivable : MonoBehaviour, IActivable
{
    public event IActivable.ActivateEvent OnActivated;
    public event IActivable.ActivateEvent OnDesactivated;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent(out ACharacter character))
            Activate();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out ACharacter character))
            Desactivate();
    }

    public void Activate()
    {
        OnActivated?.Invoke();
    }

    public void Desactivate()
    {
        OnDesactivated?.Invoke();
    }
}
