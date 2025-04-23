using System;
using System.Collections;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class ParentMoveState<TStateEnum> : BaseStatePawn<TStateEnum>
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

    public override void InitState(StateMachinePawn<TStateEnum, BaseStatePawn<TStateEnum>> stateMachine, TStateEnum enumValue, APawn<TStateEnum> character)
    {
        base.InitState(stateMachine, enumValue, character);
    }

    public override void EnterState()
    {
        base.EnterState();

        _character.InputManager.OnInteract += OnInteract;

        _cam = GameManager.Instance.CameraHandler;

        _lastDirection = _character.transform.localEulerAngles;
        _targetDirection = _character.transform.localEulerAngles;
        _startDirection = _character.transform.localEulerAngles;

        _clock = 0;
    }

    public override void ExitState()
    {
        base.ExitState();

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

        if (_lastDirection != _moveDirection && _moveDirection != Vector3.zero)
        {
            _startDirection = _lastDirection;
            _lastDirection = _moveDirection;
            _targetDirection = _moveDirection;
            _animClock = 0;

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
