using UnityEngine;

public class Box : MonoBehaviour, IMovable
{
    public float MoveSpeed { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public float OffsetRadius { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public event IMovable.NoArgVoid OnMoveFinished;

    void Start()
    {

    }

    void Update()
    {

    }

    public void Hold()
    {
        throw new System.NotImplementedException();
    }

    public void Interactable()
    {
        throw new System.NotImplementedException();
    }

    public void Move(Vector2 direction)
    {
        throw new System.NotImplementedException();
    }

    public bool Move(Vector3 direction)
    {
        throw new System.NotImplementedException();
    }
}
