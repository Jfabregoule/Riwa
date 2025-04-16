using UnityEngine;

public class MoveStateSoul : ParentMoveState<EnumStateSoul>
{
    new protected ASoul _character;

    public override void InitState(StateMachinePawn<EnumStateSoul, BaseStatePawn<EnumStateSoul>> stateMachine, EnumStateSoul enumValue, APawn<EnumStateSoul> Soul)
    {
        base.InitState(stateMachine, enumValue, Soul);

        _character = (ASoul)Soul;

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

        Vector3 targetPosition = _character.Rb.position + _moveDirection * _character.Speed * Time.fixedDeltaTime;
        Vector3 toPlayer = _character.Character.transform.position - targetPosition;
        float distanceToPlayer = toPlayer.magnitude;

        if (distanceToPlayer > _character.LinkMaxDistance)
        {
            Vector3 pullForce = toPlayer.normalized * (distanceToPlayer - _character.LinkMaxDistance) * _character.LinkElasticity;
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
