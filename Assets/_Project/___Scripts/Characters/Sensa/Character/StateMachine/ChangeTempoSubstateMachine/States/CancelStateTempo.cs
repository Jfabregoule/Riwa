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

        if (GameManager.Instance.CurrentTemporality == EnumTemporality.Present)
            Helpers.Camera.cullingMask |= 1 << 6;
        else
            Helpers.Camera.cullingMask |= 1 << 7;
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

    private void TimeChangeAborted(EnumTemporality temporality)
    {
        int layermaskToHide = temporality == EnumTemporality.Present ? 6 : 7;
        Helpers.Camera.cullingMask &= ~(1 << layermaskToHide);
        _changedTime = true;
    }
}