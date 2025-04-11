using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public interface IMovable : IHoldable
{
    public float MoveSpeed { get; set;}
    void Move(Vector3 direction);
}