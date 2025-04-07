using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CancelStateTempo : ChangeTempoBaseState
{
    private bool _changedTime = false;

    public override void InitState(ChangeTempoStateMachine stateMachine, EnumChangeTempo enumValue, ACharacter character)
    {
        base.InitState(stateMachine, enumValue, character);
    }

    public override void EnterState()
    {
        base.EnterState();

        _character.ChangeTime.AbortChangeTime();
        _character.ChangeTime.OnTimeChangeEnd += TimeChangeEnded;
    }

    public override void ExitState()
    {
        base.ExitState();

        _changedTime = false;
        _character.IsChangingTime = false;

        _character.ChangeTime.OnTimeChangeEnd -= TimeChangeEnded;
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (_changedTime)
        {
            _stateMachine.ChangeState(_stateMachine.States[EnumChangeTempo.Standby]);
        }
    }

    public override void CheckChangeState()
    {
        base.CheckChangeState();
    }

    private void TimeChangeEnded()
    {
        _changedTime = true;
    }
}
