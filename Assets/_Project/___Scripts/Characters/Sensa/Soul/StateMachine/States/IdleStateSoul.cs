using UnityEngine;

public class IdleStateSoul : PawnIdleState<EnumStateSoul>
{
    private ACharacter _sensa;

    public override void InitState(StateMachinePawn<EnumStateSoul, BaseStatePawn<EnumStateSoul>> stateMachine, EnumStateSoul enumValue, APawn<EnumStateSoul> Soul)
    {
        base.InitState(stateMachine, enumValue, Soul);
        _sensa = GameManager.Instance.Character;
    }

    public override void EnterState()
    {
        base.EnterState();

        _character.InputManager.OnInteract += OnInteract;

    }

    public override void ExitState()
    {
        base.ExitState();

        _character.InputManager.OnInteract -= OnInteract;

    }

    public override void UpdateState()
    {
        base.UpdateState();

        ASoul soul = (ASoul)_character;

        Vector3 movement;
        movement.x = _character.InputManager.GetMoveDirection().x;
        movement.y = 0;
        movement.z = _character.InputManager.GetMoveDirection().y;

        Vector3 targetPosition = _character.Rb.position + movement * _character.Speed * Time.fixedDeltaTime;
        Vector3 toPlayer = _sensa.transform.position - targetPosition;
        float distanceToPlayer = toPlayer.magnitude;

        if (distanceToPlayer > soul.LinkMaxDistance)
        {
            Vector3 pullForce = toPlayer.normalized * (distanceToPlayer - soul.LinkMaxDistance) * soul.LinkElasticity;
            _character.Rb.velocity += pullForce * Time.fixedDeltaTime;
        }

        _character.Rb.velocity = Vector3.Lerp(_character.Rb.velocity, movement * _character.Speed, Time.fixedDeltaTime * 10f);

    }

    public override void CheckChangeState()
    {
        base.CheckChangeState();

        ASoul soul = (ASoul)_character;

        if (_character.InputManager.GetMoveDirection().y != 0 || _character.InputManager.GetMoveDirection().x != 0)
        {
            _stateMachine.ChangeState(_stateMachine.States[EnumStateSoul.Move]);
            return;
        }

        if (_clock > soul.TimeBeforeWait)
        {
            //_stateMachine.ChangeState(_stateMachine.States[EnumStateSoul.Wait]);
            return;
        }
    }

    private void OnInteract()
    { 
        _stateMachine.ChangeState(_stateMachine.States[EnumStateSoul.Interact]);
    }

}
