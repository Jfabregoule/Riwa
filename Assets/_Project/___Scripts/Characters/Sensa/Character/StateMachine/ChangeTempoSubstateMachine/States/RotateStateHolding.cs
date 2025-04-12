public class RotateStateHolding : HoldingBaseState
{
    private int _sens;

    private IRotatable _rotatable;
    public int Sens { get => _sens; set => _sens = value; }
    public override void InitState(HoldingStateMachine stateMachine, EnumHolding enumValue, ACharacter character)
    {
        base.InitState(stateMachine, enumValue, character);
    }

    public override void EnterState()
    {
        base.EnterState();

        if(_character.HoldingObject.TryGetComponent(out IRotatable rotatable))
        {
            _rotatable = rotatable;
            rotatable.Rotate(Sens);
            rotatable.OnRotateFinished += Finish;
        }
    }

    public override void ExitState()
    {
        base.ExitState();

        _rotatable.OnRotateFinished -= Finish;
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }

    public override void CheckChangeState()
    {
        base.CheckChangeState();

    }

    private void Finish()
    {
        _stateMachine.ChangeState(_stateMachine.States[EnumHolding.IdleHolding]);
    }
}
