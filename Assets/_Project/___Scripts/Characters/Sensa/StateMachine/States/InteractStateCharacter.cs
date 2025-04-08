using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractStateCharacter : BaseStateCharacter
{

    private readonly List<GameObject> _colliderList = new();
    private float _currentOffset;

    private InteractStateMachine _subStateMachine;

    public override void InitState(StateMachineCharacter stateMachine, EnumStateCharacter enumValue, ACharacter character)
    {
        base.InitState(stateMachine, enumValue, character);
        _subStateMachine = new();
        _subStateMachine.InitStateMachine(character);
        _subStateMachine.InitState(_subStateMachine.States[EnumInteract.StandBy]);
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
