using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldingStateCharacter : BaseStateCharacter
{
    /// <summary>
    /// State dans lequel Sensa va tenir un gros objet
    /// 
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
    }

    public override void ChangeState()
    {
        base.ChangeState();

        //A definir si on maintient appuye ou si on toggle pour retourner en idle
    }
}
