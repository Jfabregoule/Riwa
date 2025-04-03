using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulWalkStateCharacter : BaseStateCharacter
{
    /// <summary>
    /// State ou le joueur est en état ame
    /// </summary>

    public override void InitState(StateMachineCharacter stateMachine, EnumStateCharacter enumValue, ACharacter character)
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

    public override void UpdateState(float dT)
    {
        base.UpdateState(dT);

        //Si le joueur est trop loin de son corps on le stop

    }

    public override void CheckChangeState()
    {
        base.CheckChangeState();

    }
}
