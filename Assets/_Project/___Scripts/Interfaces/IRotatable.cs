using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRotatable : IHoldable
{
    void Rotate(float angle);
}
