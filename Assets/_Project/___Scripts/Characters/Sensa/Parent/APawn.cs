using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class APawn<TStateEnum> : MonoBehaviour
    where TStateEnum : Enum
{
    //Envie de mourir a cause de nattan

    protected Rigidbody _rb;
    protected InputManager _inputManager;
    protected CapsuleCollider _capsuleCollider;

    protected StateMachinePawn<TStateEnum, BaseStatePawn<TStateEnum>> _stateMachine;

    [SerializeField] protected float _speed = 7;

    [SerializeField] protected LayerMask _pastLayer;
    [SerializeField] protected LayerMask _presentLayer;

    private bool isInPast = false;

    public delegate void PawnDelegate();
    public event PawnDelegate OnMoveToFinished;

    public Rigidbody Rb { get => _rb; }
    public InputManager InputManager { get => _inputManager; }
    public float Speed { get => _speed; set => _speed = value; }
    public CapsuleCollider CapsuleCollider { get => _capsuleCollider; set => _capsuleCollider = value; }
    public StateMachinePawn<TStateEnum, BaseStatePawn<TStateEnum>> StateMachine { get => _stateMachine; set => _stateMachine = value; }
    public LayerMask PastLayer { get => _pastLayer; }
    public LayerMask PresentLayer { get => _presentLayer; }
    public bool IsInPast { get => isInPast; set => isInPast = value; }

    public void MoveTo(Vector3 position)
    {
        StartCoroutine(CoroutineMoveTo(transform.position, position));
    }

    private IEnumerator CoroutineMoveTo(Vector3 startPos, Vector3 targetPos)
    {
        float clock = 0;

        targetPos.y = startPos.y;

        Vector3 direction = (targetPos - startPos).normalized;
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        while (clock < 1)
        {

            transform.position = Vector3.Lerp(startPos, targetPos, clock);
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, Mathf.Clamp01(clock * 3)); //Pour que sensa se tourne plus vite au début 

            clock += Time.deltaTime;

            yield return null;
        }

        OnMoveToFinished?.Invoke();

    }

}
