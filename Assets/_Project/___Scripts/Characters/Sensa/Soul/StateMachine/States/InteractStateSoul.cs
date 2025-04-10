using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractStateSoul : ParentInteractState<EnumStateSoul>
{
    new protected ASoul _character;

    public override void InitState(StateMachinePawn<EnumStateSoul,BaseStatePawn<EnumStateSoul>> stateMachine, EnumStateSoul enumValue, APawn<EnumStateSoul> Soul)
    {
        base.InitState(stateMachine, enumValue, Soul);

        _character = (ASoul)Soul;

        _subStateMachine = new SoulInteractSubstateMachine();
        _subStateMachine.InitStateMachine();
        _subStateMachine.InitState(_subStateMachine.States[EnumInteract.StandBy]);

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

    public override void FixedUpdateState()
    {
        base.FixedUpdateState();
    }

    public override void CheckChangeState()
    {
        base.CheckChangeState();
    }
}
