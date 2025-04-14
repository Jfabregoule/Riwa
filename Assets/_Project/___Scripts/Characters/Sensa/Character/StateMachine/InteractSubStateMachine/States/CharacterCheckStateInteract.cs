using System.Collections.Generic;
using UnityEngine;

public class CharacterCheckStateInteract : PawnCheckStateInteract<EnumStateCharacter>
{
    public override void InitState(PawnInteractSubstateMachine<EnumStateCharacter> stateMachine, EnumInteract enumValue, APawn<EnumStateCharacter> character)
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
    }

    public override void CheckChangeState()
    {
        base.CheckChangeState();
    }

    public override void ChangeStateToIdle()
    {
        StateMachineCharacter stateMachine = (StateMachineCharacter)_character.StateMachine;
        stateMachine.ChangeState(stateMachine.States[EnumStateCharacter.Idle]);
    }

    public override void ChangeStateToMove()
    {
        StateMachineCharacter stateMachine = (StateMachineCharacter)_character.StateMachine;
        stateMachine.ChangeState(stateMachine.States[EnumStateCharacter.Move]);
    }



}
