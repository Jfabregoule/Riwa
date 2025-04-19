using UnityEngine;

public class MoveStateSoul : ParentMoveState<EnumStateSoul>
{
    public override void InitState(StateMachinePawn<EnumStateSoul, BaseStatePawn<EnumStateSoul>> stateMachine, EnumStateSoul enumValue, APawn<EnumStateSoul> Soul)
    {
        base.InitState(stateMachine, enumValue, Soul);

    }

    public override void EnterState()
    {
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }

    public override void FixedUpdateState()
    {
        base.FixedUpdateState();

        ASoul soul = (ASoul)_character;

        Vector3 targetPosition = _character.Rb.position + _moveDirection * _character.Speed * Time.fixedDeltaTime;
        Vector3 toPlayer = soul.Character.transform.position - targetPosition;
        float distanceToPlayer = toPlayer.magnitude;

        _character.Rb.velocity = Vector3.Scale(_character.Rb.velocity, new Vector3(1, 0, 1));
        
        if (distanceToPlayer > soul.LinkMaxDistance)
        {
            Vector3 pullForce = toPlayer.normalized * (distanceToPlayer - soul.LinkMaxDistance) * soul.LinkElasticity;
            _character.Rb.velocity += pullForce * Time.fixedDeltaTime;
        }
    }

    public override void CheckChangeState()
    {
        base.CheckChangeState();

        //Vector2 direction = _character.InputManager.GetMoveDirection();
        //float magnitude = direction.magnitude;

        if (_character.InputManager.GetMoveDirection() == Vector2.zero)
        {
            _stateMachine.ChangeState(_stateMachine.States[EnumStateSoul.Idle]);
        }

    }

    protected override void OnInteract()
    {
        _stateMachine.ChangeState(_stateMachine.States[EnumStateSoul.Interact]);
    }

}
