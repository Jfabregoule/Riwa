using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public enum EnumChangeTempo
{
    Check,
    Process,
    Cancel
}

public class ChangeTempoBaseState : BaseState<EnumChangeTempo>
{
    new protected ChangeTempoStateMachine _stateMachine;
    protected ACharacter _character;

    public virtual void InitState(EnumChangeTempo enumValue, ChangeTempoStateMachine stateMachine, ACharacter character)
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
        ChangeState();
    }

    public override void ChangeState()
    {
        base.ChangeState();
    }

}
