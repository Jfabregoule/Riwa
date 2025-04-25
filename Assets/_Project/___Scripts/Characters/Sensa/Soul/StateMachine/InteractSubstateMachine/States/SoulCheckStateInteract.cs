using System;
using System.Collections.Generic;
using UnityEngine;

public class SoulCheckStateInteract : PawnCheckStateInteract<EnumStateSoul>
{
    public override void InitState(PawnInteractSubstateMachine<EnumStateSoul> stateMachine, EnumInteract enumValue, APawn<EnumStateSoul> character)
    {
        base.InitState(stateMachine, enumValue, character);

        _possibleTypes = new List<Type>(){
            typeof(ITreeStump),
            typeof(IInteractableSoul),
        };


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
        ASoul chara = (ASoul)_character;
        chara.StateMachine.ChangeState(chara.StateMachine.States[EnumStateSoul.Idle]);
    }

    public override void ChangeStateToMove()
    {
        _subStateMachine.ChangeState(_subStateMachine.States[EnumInteract.Move]);
    }

}

