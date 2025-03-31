using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulWalkStateCharacter : BaseStateCharacter
{
    /// <summary>
    /// State ou le joueur est en état ame
    /// </summary>

    public override void InitState(FSMCharacter stateMachine, Character character)
    {
        base.InitState(stateMachine, character);

        _enumState = EnumStateCharacter.SoulWalk;

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

        //Si le joueur est trop loin de son corps on le stop

    }

    public override void ChangeState()
    {
        base.ChangeState();

    }
}
