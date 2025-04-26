using UnityEngine;

public class MoveStateCharacter : PawnMoveState<EnumStateCharacter>
{
    ACharacter _chara;

    public override void InitState(StateMachinePawn<EnumStateCharacter, BaseStatePawn<EnumStateCharacter>> stateMachine, EnumStateCharacter enumValue, APawn<EnumStateCharacter> character)
    {
        base.InitState(stateMachine, enumValue, character);
        _chara = (ACharacter)character;
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
    }

    public override void CheckChangeState()
    {
        base.CheckChangeState();

        if (_chara.IsChangingTime)
        {
            _stateMachine.ChangeState(_stateMachine.States[EnumStateCharacter.ChangeTempo]);
        }

        else if (_chara.InputManager.GetMoveDirection() == Vector2.zero)
        {
            _stateMachine.ChangeState(_stateMachine.States[EnumStateCharacter.Idle]);
        }

        if (!_chara.Feet.IsGround)
        {
            GoToFall();
        }

    }

    protected override void OnInteract()
    {
        _stateMachine.ChangeState(_stateMachine.States[EnumStateCharacter.Interact]);
    }

    protected void GoToFall()
    {
        _stateMachine.ChangeState(_stateMachine.States[EnumStateCharacter.Fall]);
    }

}
