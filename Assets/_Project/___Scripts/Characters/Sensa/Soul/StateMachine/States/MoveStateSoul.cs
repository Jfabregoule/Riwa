using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class MoveStateSoul : BaseStateSoul
{

    private CameraHandler _cam;
    private Vector3 _moveDirection;

    public override void InitState(StateMachineSoul stateMachine, EnumStateSoul enumValue, ASoul Soul)
    {
        base.InitState(stateMachine, enumValue, Soul);
    }

    public override void EnterState()
    {
        base.EnterState();

        _cam = GameManager.Instance.CameraHandler;
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        Vector3 movement;

        movement.x = _soul.Joystick.Direction.x;
        movement.y = 0;
        movement.z = _soul.Joystick.Direction.y;

        Vector3 camForward = _cam.transform.forward;
        Vector3 camRight = _cam.transform.right;

        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        _moveDirection = (camForward * movement.z + camRight * movement.x);
    }

    public override void FixedUpdateState()
    {
        Vector3 targetPosition = _soul.Rb.position + _moveDirection * _soul.Speed * Time.fixedDeltaTime;
        Vector3 toPlayer = _soul.Character.transform.position - targetPosition;
        float distanceToPlayer = toPlayer.magnitude;

        if (distanceToPlayer > _soul.LinkMaxDistance)
        {
            Vector3 pullForce = toPlayer.normalized * (distanceToPlayer - _soul.LinkMaxDistance) * _soul.LinkElasticity;
            _soul.Rb.velocity += pullForce * Time.fixedDeltaTime;
        }

        _soul.Rb.velocity = Vector3.Lerp(_soul.Rb.velocity, _moveDirection * _soul.Speed, Time.fixedDeltaTime * 10f);

        if (_moveDirection != Vector3.zero)
        {
            _soul.SoulPawn.transform.forward = _moveDirection;
        }
    }

    public override void CheckChangeState()
    {
        base.CheckChangeState();

        Vector2 direction = new Vector2(_soul.Joystick.Direction.x , _soul.Joystick.Direction.y);
        float magnitude = direction.magnitude;

        if (_soul.Joystick.Direction.y == 0 && _soul.Joystick.Direction.x == 0)
        {
            _stateMachine.ChangeState(_stateMachine.States[EnumStateSoul.Idle]);
        }

    }
}
