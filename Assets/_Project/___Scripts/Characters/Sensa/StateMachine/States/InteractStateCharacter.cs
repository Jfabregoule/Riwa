using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractStateCharacter : ParentInteractState<EnumStateCharacter>
{
    private float _currentOffset;

    public void InitState(StateMachinePawn<EnumStateCharacter, BaseStatePawn<EnumStateCharacter>> stateMachine, EnumStateCharacter enumValue, ACharacter character)
    {
        base.InitState(stateMachine, enumValue, character);
        
    }

    public override void EnterState()
    {
        base.EnterState();

        _subStateMachine.ChangeState(_subStateMachine.States[EnumInteract.Check]);
    }

    public override void ExitState()
    {
        base.ExitState();

        _subStateMachine.InitState(_subStateMachine.States[EnumInteract.StandBy]);

    }

    public override void UpdateState()
    {
        base.UpdateState();
        _subStateMachine.StateMachineUpdate();

    }

    public override void CheckChangeState()
    {
        base.CheckChangeState();

    }    
}
