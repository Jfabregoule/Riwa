using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulActionStateInteract : PawnActionStateInteract<EnumStateSoul>
{
    float _animClock;

    public override void InitState(PawnInteractSubstateMachine<EnumStateSoul> stateMachine, EnumInteract enumValue, APawn<EnumStateSoul> character)
    {
        base.InitState(stateMachine, enumValue, character);
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

        _animClock += Time.deltaTime;

    }

    public override void CheckChangeState()
    {
        base.CheckChangeState();

        if (_animClock > 1) //Temps d'animation
        {
            _character.StateMachine.ChangeState(_character.StateMachine.States[EnumStateSoul.Idle]);
        }
    }
}
