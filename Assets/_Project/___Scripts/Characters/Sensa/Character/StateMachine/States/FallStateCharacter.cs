using UnityEngine;

public class FallStateCharacter : BaseStateCharacter<EnumStateCharacter>
{
    public override void InitState(StateMachinePawn<EnumStateCharacter, BaseStatePawn<EnumStateCharacter>> stateMachine, EnumStateCharacter enumValue, APawn<EnumStateCharacter> character)
    {
        base.InitState(stateMachine, enumValue, character);
    }

    public override void EnterState()
    {
        base.EnterState();

        ACharacter chara = (ACharacter)_character;
        chara.Feet.OnGround += GoToIdle;

    }

    public override void ExitState()
    {
        base.ExitState();
        ACharacter chara = (ACharacter)_character;
        chara.Feet.OnGround -= GoToIdle;
    }

    public override void UpdateState()
    {
        base.UpdateState();
        Debug.Log("FALL");
    }

    public override void CheckChangeState()
    {
        base.CheckChangeState();
    }

    public void GoToIdle()
    {
        _stateMachine.ChangeState(_stateMachine.States[EnumStateCharacter.Idle]);
    }

}
