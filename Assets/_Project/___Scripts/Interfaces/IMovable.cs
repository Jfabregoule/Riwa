using UnityEngine;

public interface IMovable : IHoldable
{
    public float MoveSpeed { get; set;}
    void Move(Vector3 direction);
}