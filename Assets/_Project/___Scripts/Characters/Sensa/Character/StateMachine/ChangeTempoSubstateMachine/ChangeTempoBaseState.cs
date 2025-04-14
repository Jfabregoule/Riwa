public enum EnumChangeTempo
{
    Standby,
    Check,
    Process,
    Cancel
}

public class ChangeTempoBaseState : BaseState<EnumChangeTempo>
{
    new protected ChangeTempoStateMachine _stateMachine;
    protected ACharacter _character;

    public virtual void InitState(ChangeTempoStateMachine stateMachine, EnumChangeTempo enumValue, ACharacter character)
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
