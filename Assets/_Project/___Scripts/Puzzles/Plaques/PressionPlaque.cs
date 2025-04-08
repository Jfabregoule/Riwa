using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressionPlaque : MonoBehaviour, IActivable
{
    public event IActivable.ActivateEvent OnActivated;
    public event IActivable.ActivateEvent OnDesactivated;

    void Start()
    {

    }

    void Update()
    {
        
    }

    public void Activate()
    {
        OnActivated?.Invoke();
    }

    public void Deactivate()
    {
        OnDesactivated?.Invoke();
    }
}
