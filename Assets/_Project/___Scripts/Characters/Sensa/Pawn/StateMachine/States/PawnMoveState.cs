using System;
using System.Collections;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static UnityEngine.UI.Image;

public class PawnMoveState<TStateEnum> : BaseStatePawn<TStateEnum>
    where TStateEnum : Enum
{
    protected CameraHandler _cam;
    protected Vector3 _moveDirection;

    protected Vector3 _lastDirection;
    protected Vector3 _startDirection;
    protected Vector3 _targetDirection;
    protected float _animClock = 0;

    protected float _currentSpeed;
    protected float _clock;
    protected float _acceleration = 0.5f;

    protected float _checkStepDistance;

    public override void InitState(StateMachinePawn<TStateEnum, BaseStatePawn<TStateEnum>> stateMachine, TStateEnum enumValue, APawn<TStateEnum> character)
    {
        base.InitState(stateMachine, enumValue, character);
        
        _checkStepDistance = _character.GetComponent<CapsuleCollider>().radius * _character.transform.localScale.x * 1.2f;
    }

    public override void EnterState()
    {
        base.EnterState();

        _character.InputManager.OnInteract += OnInteract;

        _cam = GameManager.Instance.CameraHandler;

        _lastDirection = _character.transform.forward;
        _targetDirection = _character.transform.forward;
        _startDirection = _character.transform.forward;

        _clock = 0;
    }

    public override void ExitState()
    {
        base.ExitState();

        _character.InputManager.OnInteract -= OnInteract;
    }

    public override void DestroyState()
    {
        _character.InputManager.OnInteract -= OnInteract;
    }

    public override void UpdateState()
    {
        base.UpdateState();

        
        Vector2 direction = _character.InputManager.GetMoveDirection();

        Vector3 camForward = _cam.transform.forward;
        Vector3 camRight = _cam.transform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward.Normalize();
        camRight.Normalize();
        _moveDirection = (camForward * direction.y + camRight * direction.x);
        _moveDirection.y = 0;

        if (_lastDirection != _moveDirection && _moveDirection != Vector3.zero)
        {
            _startDirection = _lastDirection;
            _lastDirection = _moveDirection;
            _targetDirection = _moveDirection;
            _targetDirection.y = 0;
            _animClock = 0;

        }

        Vector3 lowerRayOrigin = _character.transform.position + Vector3.up * 0.1f;
        Vector3 upperRayOrigin = _character.transform.position + Vector3.up * (_character.StepHeight + 0.1f);

        bool lowerHit = Physics.Raycast(lowerRayOrigin, _moveDirection, out RaycastHit lowerHitInfo, _checkStepDistance);
        bool upperHit = Physics.Raycast(upperRayOrigin, _moveDirection, _checkStepDistance);

        Debug.DrawRay(lowerRayOrigin, _moveDirection * _checkStepDistance, Color.red);
        Debug.DrawRay(upperRayOrigin, _moveDirection * _checkStepDistance, Color.cyan);

        if (lowerHit && !upperHit)
        {
                _character.Rb.position += Vector3.up * _character.StepHeight;
        }

        _animClock += Time.deltaTime * 20;
        //_character.transform.forward = Vector3.Lerp(_startDirection, _targetDirection, _animClock);

        Quaternion targetRotation = Quaternion.LookRotation(_targetDirection);
        _character.transform.rotation = Quaternion.Slerp(
            _character.transform.rotation,
            targetRotation,
            _animClock
        );


        _character.Animator.SetFloat("MagnitudeVel", Vector3.Magnitude(_moveDirection));

        //Lerp Speed

        //_currentSpeed = _character.Speed;

        _clock += Time.deltaTime;
        _clock = Mathf.Clamp(_clock, 0, _acceleration);
        if (_acceleration == 0) { throw new Exception("Accel doit etre different de 0"); }
        _currentSpeed = Mathf.Lerp(0, _character.Speed, _clock / _acceleration);

    }

    public override void FixedUpdateState()
    {
        base.FixedUpdateState();

        _character.Rb.velocity = _moveDirection * _currentSpeed + Vector3.Scale(_character.Rb.velocity, Vector3.up);

    }

    public override void CheckChangeState()
    {
        base.CheckChangeState();
    }

    protected virtual void OnInteract() { }

}
