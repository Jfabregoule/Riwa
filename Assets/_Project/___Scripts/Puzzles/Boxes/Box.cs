using System.Collections;
using UnityEngine;

public class Box : MonoBehaviour, IMovable
{
    public float MoveSpeed { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public float OffsetRadius { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    float IMovable.MoveDistance { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public bool CanInteract { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public int Priority { get; set; }

    public event IMovable.NoArgVoid OnMoveFinished;
    public event IMovable.NoArgVector3 OnReplacePlayer;

    void Start()
    {
        Priority = 0;
    }

    void Update()
    {

    }

    public void Hold()
    {
        throw new System.NotImplementedException();
    }

    public void Interact()
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

    IEnumerator IMovable.MoveLerp(Vector3 direction)
    {
        throw new System.NotImplementedException();
    }
}
