using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallStateCharacter : BaseStateCharacter
{
    public override void InitState(FSMCharacter stateMachine, Character character)
    {
        base.InitState(stateMachine, character);

        _enumState = EnumStateCharacter.Fall;
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
    }
}
