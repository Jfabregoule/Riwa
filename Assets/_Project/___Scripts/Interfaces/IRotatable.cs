using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRotatable : IHoldable
{
    void Rotate(int sens);

    public delegate void RotatableEvent();
    event RotatableEvent OnRotateFinished;
}
