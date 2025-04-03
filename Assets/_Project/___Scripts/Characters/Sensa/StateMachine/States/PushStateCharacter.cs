using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushStateCharacter : BaseStateCharacter
{
    /// <summary>
    /// Lorsque le joueur est en holding, il peut pousser l'objet 
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

    public override void CheckChangeState()
    {
        base.CheckChangeState();

        //Si on ne d�sactive pas le holding et qu'on lache le joystick
        _stateMachine.ChangeState(_stateMachine.States[EnumStateCharacter.Holding]);

        //Si d�sactive le joystick et qu'on a le joystick braqu� 
        _stateMachine.ChangeState(_stateMachine.States[EnumStateCharacter.Move]);

    }
}
