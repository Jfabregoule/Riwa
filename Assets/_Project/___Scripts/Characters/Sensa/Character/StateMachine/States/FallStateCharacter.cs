using System.Collections;
using UnityEngine;

public class FallStateCharacter : BaseStateCharacter<EnumStateCharacter>
{

    ACharacter _chara;
    private bool _canTriggerLanding;

    public override void InitState(StateMachinePawn<EnumStateCharacter, BaseStatePawn<EnumStateCharacter>> stateMachine, EnumStateCharacter enumValue, APawn<EnumStateCharacter> character)
    {
        base.InitState(stateMachine, enumValue, character);
        _chara = (ACharacter)_character;
    }

    public override void EnterState()
    {
        base.EnterState();

        //_character.Rb.velocity = new Vector3(0,_character.Rb.velocity.y,0);

        //_chara.Feet.OnGround += GoToIdle;

        _canTriggerLanding = true;
    }

    public override void ExitState()
    {
        base.ExitState();
        
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }

    public override void CheckChangeState()
    {
        base.CheckChangeState();

        if (_chara.Feet.IsGround && _canTriggerLanding)
        {
            ACharacter chara = (ACharacter)_chara;
            _stateMachine.ChangeState(_stateMachine.States[EnumStateCharacter.Idle]);
            _canTriggerLanding = false;
        }
    }
}
