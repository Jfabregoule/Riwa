using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleStateCharacter : BaseStateCharacter
{

    public override void InitState(FSMCharacter stateMachine, Character character)
    {
        _stateMachine = stateMachine;
        _enumState = EnumStateCharacter.Idle;    
        
        //Pas de transition
    }

    public override void EnterState() { 
    
    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {
        ChangeState();
    }

    public override void ChangeState()
    {
        if (_character.Joystick.Direction.y != 0 || _character.Joystick.Direction.x != 0)
        {
            _stateMachine.ChangeState(_stateMachine._states[EnumStateCharacter.Walk]);
        }
    }
}
