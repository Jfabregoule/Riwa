using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public interface IMovable : IHoldable
{
    void Move(Vector2 direction);
}