using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnumSoul
{
    IdleSoul, 
    MoveSoul,
}

public class SoulBaseState : BaseState<EnumSoul>
{
    new protected SoulStateMachine _stateMachine;
    protected ACharacter _character;

    public virtual void InitState(SoulStateMachine stateMachine, EnumSoul enumValue, ACharacter character)
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

    public override void UpdateState(float dT)
    {
        base.UpdateState(dT);
        CheckChangeState();
    }

    public override void CheckChangeState()
    {
        base.CheckChangeState();
    }
}
