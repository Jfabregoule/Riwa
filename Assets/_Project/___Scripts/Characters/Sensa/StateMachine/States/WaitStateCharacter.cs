using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitStateCharacter : BaseStateCharacter
{
    /// <summary>
    /// State ou le joueur aura les inputs bloqués pour les cinématiques
    /// pourra surement prendre en paramètre un sequencer, et jouera ce sequncer dans ce state
    /// </summary>

    public override void InitState(FSMCharacter stateMachine, Character character)
    {
        base.InitState(stateMachine, character);

        _enumState = EnumStateCharacter.Wait;
    }

    public override void EnterState()
    {
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void UpdateState(float dT)
    {
        base.UpdateState(dT);
    }

    public override void ChangeState()
    {
        base.ChangeState();

        if (_character.Joystick.Direction.y != 0 || _character.Joystick.Direction.x != 0)
        {
            _stateMachine.ChangeState(_stateMachine.States[EnumStateCharacter.Walk]);
        }
    }
}
