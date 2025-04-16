using System;
using UnityEngine;

public class ParentMoveState<TStateEnum> : BaseStatePawn<TStateEnum>
    where TStateEnum : Enum
{
    protected CameraHandler _cam;
    protected Vector3 _moveDirection;

    public override void InitState(StateMachinePawn<TStateEnum, BaseStatePawn<TStateEnum>> stateMachine, TStateEnum enumValue, APawn<TStateEnum> character)
    {
        base.InitState(stateMachine, enumValue, character);
    }

    public override void EnterState()
    {
        base.EnterState();

        _character.InputManager.OnInteract += OnInteract;

        _cam = GameManager.Instance.CameraHandler;
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
    }

    public override void FixedUpdateState()
    {
        base.FixedUpdateState();

        _character.Rb.velocity = _moveDirection * _character.Speed + Vector3.Scale(_character.Rb.velocity, Vector3.up);

        if (_moveDirection != Vector3.zero)
        {
            _character.transform.forward = _moveDirection;
        }
    }

    public override void CheckChangeState()
    {
        base.CheckChangeState();
    }

    protected virtual void OnInteract() { }

}
