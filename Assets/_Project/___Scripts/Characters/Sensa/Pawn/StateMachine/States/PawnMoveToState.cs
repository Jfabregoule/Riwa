using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnMoveToState<TStateEnum> : BaseStatePawn<TStateEnum>
    where TStateEnum : Enum
{
    protected Vector3 _targetPos;
    protected Vector3 _objectPos;
    protected bool _endRotate;

    protected TStateEnum _nextState;

    public override void InitState(StateMachinePawn<TStateEnum, BaseStatePawn<TStateEnum>> stateMachine, TStateEnum enumValue, APawn<TStateEnum> character)
    {
        base.InitState(stateMachine, enumValue, character);
    }

    public void LoadState(TStateEnum nextState, Vector3 targetPos, Vector3 objectPos, bool endRotate = true)
    {
        _targetPos = targetPos;
        _objectPos = objectPos;
        _endRotate = endRotate; 

        _nextState = nextState;

    }

    public override void EnterState()
    {
        base.EnterState();

        _character.MoveTo(_targetPos, _objectPos, _endRotate);

        _character.OnMoveToFinished += GoToNextState;
    }

    public override void ExitState()
    {
        base.ExitState();
        _character.OnMoveToFinished -= GoToNextState;
    }

    public override void DestroyState()
    {
        _character.OnMoveToFinished -= GoToNextState;
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }

    public override void FixedUpdateState()
    {
        base.FixedUpdateState();
    }

    public override void CheckChangeState()
    {
        base.CheckChangeState();
    }

    protected virtual void OnInteract() { }

    public void GoToNextState()
    {
        _character.StateMachine.ChangeState(_character.StateMachine.States[_nextState]);
    }

}
