using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCheckStateInteract : PawnCheckStateInteract<EnumStateCharacter>
{
    public override void InitState(PawnInteractSubstateMachine<EnumStateCharacter> stateMachine, EnumInteract enumValue, APawn<EnumStateCharacter> character)
    {
        base.InitState(stateMachine, enumValue, character);

        _possibleTypes = new List<Type>(){
            typeof(IHoldable),
            typeof(IMovable),
            typeof(IRotatable),
            typeof(ITreeStump),
            typeof(IInteractable)
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
        ACharacter chara = (ACharacter)_character;
        chara.StateMachine.ChangeState(chara.StateMachine.States[EnumStateCharacter.Idle]);
    }

    public override void ChangeStateToMove()
    {
        _subStateMachine.ChangeState(_subStateMachine.States[EnumInteract.Move]);
    }



}
