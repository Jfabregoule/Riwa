using UnityEngine;

public class FallStateCharacter : BaseStateCharacter<EnumStateCharacter>
{

    ACharacter _chara;

    public override void InitState(StateMachinePawn<EnumStateCharacter, BaseStatePawn<EnumStateCharacter>> stateMachine, EnumStateCharacter enumValue, APawn<EnumStateCharacter> character)
    {
        base.InitState(stateMachine, enumValue, character);
        _chara = (ACharacter)_character;
    }

    public override void EnterState()
    {
        base.EnterState();

        //_chara.Feet.OnGround += GoToIdle;

    }

    public override void ExitState()
    {
        base.ExitState();
        //_chara.Feet.OnGround -= GoToIdle;
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }

    public override void CheckChangeState()
    {
        base.CheckChangeState();

        if (_chara.Feet.IsGround)
        {
            _stateMachine.ChangeState(_stateMachine.States[EnumStateCharacter.Idle]);
        }


    }

}
