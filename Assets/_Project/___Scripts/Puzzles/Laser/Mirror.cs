using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour, IRotatable
{
    public float OffsetRadius { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public event IRotatable.RotatableEvent OnRotateFinished;

    public void Hold()
    {
        throw new System.NotImplementedException();
    }

    public void Interactable()
    {
        throw new System.NotImplementedException();
    }

    public void Rotate(float angle)
    {
        OnRotateFinished?.Invoke();
        throw new System.NotImplementedException();
    }

    public void Rotate(int sens)
    {
        throw new System.NotImplementedException();
    }
}
