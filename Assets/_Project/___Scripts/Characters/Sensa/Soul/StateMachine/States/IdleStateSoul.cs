using UnityEngine;

public class IdleStateSoul : ParentIdleState<EnumStateSoul>
{
    private ACharacter _sensa;
    new protected ASoul _character;

    public override void InitState(StateMachinePawn<EnumStateSoul, BaseStatePawn<EnumStateSoul>> stateMachine, EnumStateSoul enumValue, APawn<EnumStateSoul> Soul)
    {
        base.InitState(stateMachine, enumValue, Soul);
        _sensa = GameManager.Instance.Character;
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

        Vector3 movement;
        movement.x = _character.InputManager.GetMoveDirection().x;
        movement.y = 0;
        movement.z = _character.InputManager.GetMoveDirection().y;

        Vector3 targetPosition = _character.Rb.position + movement * _character.Speed * Time.fixedDeltaTime;
        Vector3 toPlayer = _sensa.transform.position - targetPosition;
        float distanceToPlayer = toPlayer.magnitude;

        if (distanceToPlayer > _character.LinkMaxDistance)
        {
            Vector3 pullForce = toPlayer.normalized * (distanceToPlayer - _character.LinkMaxDistance) * _character.LinkElasticity;
            _character.Rb.velocity += pullForce * Time.fixedDeltaTime;
        }

        _character.Rb.velocity = Vector3.Lerp(_character.Rb.velocity, movement * _character.Speed, Time.fixedDeltaTime * 10f);

    }

    public override void CheckChangeState()
    {
        base.CheckChangeState();

        if (_character.InputManager.GetMoveDirection().y != 0 || _character.InputManager.GetMoveDirection().x != 0)
        {
            _stateMachine.ChangeState(_stateMachine.States[EnumStateSoul.Move]);
            return;
        }

        if (_clock > _character.TimeBeforeWait)
        {
            //_stateMachine.ChangeState(_stateMachine.States[EnumStateSoul.Wait]);
            return;
        }
    }
}
