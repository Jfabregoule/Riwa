using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleStateSoul : BaseStateSoul
{
    private float _clock;

    public override void InitState(StateMachineSoul stateMachine, EnumStateSoul enumValue, ASoul Soul)
    {
        base.InitState(stateMachine, enumValue, Soul);
    }

    public override void EnterState()
    {
        base.EnterState();

        _clock = 0;
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

        Vector3 targetPosition = _soul.Rb.position + movement * _soul.Speed * Time.fixedDeltaTime;
        Vector3 toPlayer = _soul.Character.transform.position - targetPosition;
        float distanceToPlayer = toPlayer.magnitude;

        if (distanceToPlayer > _soul.LinkMaxDistance)
        {
            Vector3 pullForce = toPlayer.normalized * (distanceToPlayer - _soul.LinkMaxDistance) * _soul.LinkElasticity;
            _soul.Rb.velocity += pullForce * Time.fixedDeltaTime;
        }

        _soul.Rb.velocity = Vector3.Lerp(_soul.Rb.velocity, movement * _soul.Speed, Time.fixedDeltaTime * 10f);

    }

    public override void CheckChangeState()
    {
        base.CheckChangeState();

        if (_soul.Joystick.Direction.y != 0 || _soul.Joystick.Direction.x != 0)
        {
            _stateMachine.ChangeState(_stateMachine.States[EnumStateSoul.Move]);
            return;
        }

        if (_clock > _soul.TimeBeforeWait)
        {
            //_stateMachine.ChangeState(_stateMachine.States[EnumStateSoul.Wait]);
            return;
        }
    }
}
