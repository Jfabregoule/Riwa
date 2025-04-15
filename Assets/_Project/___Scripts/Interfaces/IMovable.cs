using UnityEngine;

public interface IMovable : IHoldable
{
    public float MoveSpeed { get; set;}
    bool Move(Vector3 direction);

    public delegate void NoArgVoid();
    public event NoArgVoid OnMoveFinished;

}