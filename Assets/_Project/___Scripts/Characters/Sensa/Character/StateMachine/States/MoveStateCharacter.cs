using UnityEngine;

public class MoveStateCharacter : ParentMoveState<EnumStateCharacter>
{
    public override void InitState(StateMachinePawn<EnumStateCharacter, BaseStatePawn<EnumStateCharacter>> stateMachine, EnumStateCharacter enumValue, APawn<EnumStateCharacter> character)
    {
        base.InitState(stateMachine, enumValue, character);
    }

    public override void EnterState()
    {
        base.EnterState();
        ACharacter chara = (ACharacter)_character;
        chara.Feet.OnFall += GoToFall;
    }

    public override void ExitState()
    {
        base.ExitState();
        ACharacter chara = (ACharacter)_character;
        chara.Feet.OnFall -= GoToFall;
        
    }

    public override void UpdateState()
    {
        base.UpdateState();

        ACharacter chara = (ACharacter)_character;

        chara.Animator.SetFloat("MagnitudeVelocity", chara.MagnitudeVelocity);

    }

    public override void FixedUpdateState()
    {
        base.FixedUpdateState();
    }

    public override void CheckChangeState()
    {
        base.CheckChangeState();

        ACharacter chara = (ACharacter)_character;

        if (chara.IsChangingTime)
        {
            _stateMachine.ChangeState(_stateMachine.States[EnumStateCharacter.ChangeTempo]);
        }

        else if (chara.InputManager.GetMoveDirection() == Vector2.zero)
        {
            _stateMachine.ChangeState(_stateMachine.States[EnumStateCharacter.Idle]);
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
