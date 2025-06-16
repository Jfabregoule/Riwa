using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class APawn<TStateEnum> : MonoBehaviour
    where TStateEnum : Enum
{
    protected Rigidbody _rb;
    protected InputManager _inputManager;
    protected CapsuleCollider _capsuleCollider;

    protected Animator _animator;

    protected StateMachinePawn<TStateEnum, BaseStatePawn<TStateEnum>> _stateMachine;

    [SerializeField] protected float _speed = 7;
    [SerializeField] private float _walkSpeed = 2;
    [SerializeField] private float interactRadius = 5;

    [SerializeField] protected LayerMask _pastLayer;
    [SerializeField] protected LayerMask _presentLayer;

    public delegate void PawnDelegate();
    public event PawnDelegate OnMoveToFinished;
    public event PawnDelegate OnInteractStarted;
    public event PawnDelegate OnInteractEnded;

    public Rigidbody Rb { get => _rb; }
    public InputManager InputManager { get => _inputManager; }
    public float Speed { get => _speed; set => _speed = value; }
    public CapsuleCollider CapsuleCollider { get => _capsuleCollider; set => _capsuleCollider = value; }
    public StateMachinePawn<TStateEnum, BaseStatePawn<TStateEnum>> StateMachine { get => _stateMachine; set => _stateMachine = value; }
    public LayerMask PastLayer { get => _pastLayer; }
    public LayerMask PresentLayer { get => _presentLayer; }
    public Animator Animator { get => _animator; set => _animator = value; }
    public float InteractRadius { get => interactRadius; set => interactRadius = value; }
    public float WalkSpeed { get => _walkSpeed; set => _walkSpeed = value; }

    public void MoveTo(Vector3 position, Vector3 objectPos, bool endRotate = true)
    {
        StartCoroutine(CoroutineMoveTo(transform.position, position, objectPos, endRotate));
    }

    private IEnumerator CoroutineMoveTo(Vector3 startPos, Vector3 targetPos, Vector3 objectPos, bool endRotate)
    {
        targetPos.y = startPos.y;

        Vector3 direction = (targetPos - startPos).normalized;
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        float distance = Vector3.Distance(startPos, targetPos);
        float duration = distance / _walkSpeed;
        float clock = 0f;

        while (clock < duration)
        {
            float t = clock / duration;

            transform.position = Vector3.Lerp(startPos, targetPos, t);
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, Mathf.Clamp01(t * 3)); //Pour que sensa se tourne plus vite au début 

            clock += Time.deltaTime;

            yield return null;
        }

        clock = 0;
        startRotation = transform.rotation;
        Vector3 lookDir = objectPos - transform.position;
        lookDir.y = 0f;
        if (lookDir != Vector3.zero)
            targetRotation = Quaternion.LookRotation(lookDir);

        if (!endRotate)
        {
            clock = 1.1f;
        }

        while (clock < 1)
        {
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, Mathf.Clamp01(clock)); //Pour que sensa se tourne plus vite au début 

            clock += Time.deltaTime * 8;
            clock = Mathf.Clamp01(clock);

            yield return null;
        }

        OnMoveToFinished?.Invoke();
}

    public virtual void OnDisable()
    {
        foreach (KeyValuePair<TStateEnum, BaseStatePawn<TStateEnum>> pair in _stateMachine.States)
        {
            pair.Value.DestroyState();
        }
    }

    public void InteractBegin()
    {
        OnInteractStarted?.Invoke();
    }

    public void InteractEnd()
    {
        OnInteractEnded?.Invoke();
    }
}
