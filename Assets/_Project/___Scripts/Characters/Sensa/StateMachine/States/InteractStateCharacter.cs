using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractStateCharacter : BaseStateCharacter
{
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

    public override void UpdateState()
    {
        base.UpdateState();
        Debug.Log("Interact");
    }

    public override void CheckChangeState()
    {
        base.CheckChangeState();

        _stateMachine.ChangeState(_stateMachine.States[EnumStateCharacter.Idle]);
    }
}
