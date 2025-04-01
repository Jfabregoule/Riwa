using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldingStateCharacter : BaseStateCharacter
{
    /// <summary>
    /// State dans lequel Sensa va tenir un gros objet
    /// 
    /// </summary>

    public override void InitState(StateMachineCharacter stateMachine, Character character)
    {
        base.InitState(stateMachine, character);

        _enumState = EnumStateCharacter.Holding;
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

        //A definir si on maintient appuye ou si on toggle pour retourner en idle
    }
}
