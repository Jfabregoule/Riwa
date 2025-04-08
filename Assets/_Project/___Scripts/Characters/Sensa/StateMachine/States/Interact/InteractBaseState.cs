using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnumInteract
{
    StandBy,
    Check,
    Action,
    Move
}

public class InteractBaseState : BaseState<EnumInteract>
{
    new protected InteractStateMachine _stateMachine;
    protected ACharacter _character;

    public virtual void InitState(InteractStateMachine stateMachine, EnumInteract enumValue, ACharacter character)
    {
        base.InitState(enumValue);

        _stateMachine = stateMachine;
        _character = character;

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

        CheckChangeState();
    }

    public override void CheckChangeState()
    {
        base.CheckChangeState();
    }
}
