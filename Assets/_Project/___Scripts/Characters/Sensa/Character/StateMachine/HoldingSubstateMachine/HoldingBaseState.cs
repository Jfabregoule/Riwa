public enum EnumHolding
{
    IdleHolding, 
    Move,
    Rotate
}

public class HoldingBaseState : BaseState<EnumHolding>
{
    new protected HoldingStateMachine _stateMachine;
    protected ACharacter _character;
    protected CameraHandler _cam;

    public virtual void InitState(HoldingStateMachine stateMachine, EnumHolding enumValue, ACharacter character)
    {
        base.InitState(enumValue);

        _stateMachine = stateMachine;
        _character = character;

        _cam = GameManager.Instance.CameraHandler;

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

        //CheckChangeState();
    }

    public override void CheckChangeState()
    {
        base.CheckChangeState();
    }
}
