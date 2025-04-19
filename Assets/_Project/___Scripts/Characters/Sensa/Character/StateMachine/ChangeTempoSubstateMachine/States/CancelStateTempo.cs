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

        GameManager.Instance.OnTimeChangeAborted += TimeChangeAborted;
        _character.ChangeTime.AbortChangeTime();
    }

    public override void ExitState()
    {
        base.ExitState();

        _changedTime = false;
        _character.IsChangingTime = false;

        GameManager.Instance.OnTimeChangeAborted -= TimeChangeAborted;
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

    private void TimeChangeAborted()
    {
        _changedTime = true;
    }
}
