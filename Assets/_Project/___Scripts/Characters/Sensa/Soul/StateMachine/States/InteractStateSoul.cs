
public class InteractStateSoul : ParentInteractState<EnumStateSoul>
{
    new protected ASoul _character;

    public override void InitState(StateMachinePawn<EnumStateSoul,BaseStatePawn<EnumStateSoul>> stateMachine, EnumStateSoul enumValue, APawn<EnumStateSoul> Soul)
    {
        base.InitState(stateMachine, enumValue, Soul);

        _character = (ASoul)Soul;

        _subStateMachine = new SoulInteractSubstateMachine();
        _subStateMachine.InitStateMachine(Soul);
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
        _subStateMachine.ChangeState(_subStateMachine.States[EnumInteract.StandBy]);
    }

    public override void UpdateState()
    {
        base.UpdateState();
        _subStateMachine.StateMachineUpdate();
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
