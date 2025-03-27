using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushStateCharacter : BaseStateCharacter
{
    /// <summary>
    /// Lorsque le joueur est en holding, il peut pousser l'objet 
    /// </summary>

    public override void InitState(FSMCharacter stateMachine, Character character)
    {
        base.InitState(stateMachine, character);
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
    }

    public override void ChangeState()
    {
        base.ChangeState();

        //Si on ne désactive pas le holding et qu'on lache le joystick
        _stateMachine.ChangeState(_stateMachine.States[EnumStateCharacter.Holding]);

        //Si désactive le joystick et qu'on a le joystick braqué 
        _stateMachine.ChangeState(_stateMachine.States[EnumStateCharacter.Walk]);

    }
}
