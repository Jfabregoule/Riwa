using UnityEngine;

public class ProcessStateTempo : ChangeTempoBaseState
{
    private bool _changedTime = false;

    public override void InitState(ChangeTempoStateMachine stateMachine, EnumChangeTempo enumValue, ACharacter character)
    {
        base.InitState(stateMachine, enumValue,character);
    }

    public override void EnterState()
    {
        base.EnterState();

        _changedTime = false;
        _character.CanChangeTime = false;

        _character.ChangeTime.StartTimeChange();
        GameManager.Instance.OnTimeChangeEnded += TimeChangeEnded;

        _character.IsInPast = !_character.IsInPast;
        _character.Soul.GetComponent<ASoul>().IsInPast = _character.IsInPast;

        if (!_character.IsInPast)
        {
            Physics.IgnoreLayerCollision(_character.gameObject.layer, Mathf.Clamp(Mathf.RoundToInt(Mathf.Log(_character.PastLayer.value, 2)), 0, 31), true);
            Physics.IgnoreLayerCollision(_character.gameObject.layer, Mathf.Clamp(Mathf.RoundToInt(Mathf.Log(_character.PresentLayer.value, 2)), 0, 31), false);
        }
        else
        {
            Physics.IgnoreLayerCollision(_character.gameObject.layer, Mathf.Clamp(Mathf.RoundToInt(Mathf.Log(_character.PresentLayer.value, 2)), 0, 31), true);
            Physics.IgnoreLayerCollision(_character.gameObject.layer, Mathf.Clamp(Mathf.RoundToInt(Mathf.Log(_character.PastLayer.value, 2)), 0, 31), false);
        }

    }

    public override void ExitState()
    {
        base.ExitState();
        _changedTime = false;
        _character.IsChangingTime = false;

        GameManager.Instance.OnTimeChangeEnded -= TimeChangeEnded;
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }

    public override void CheckChangeState()
    {
        base.CheckChangeState();

        if (_changedTime)
        {
            _stateMachine.ChangeState(_stateMachine.States[EnumChangeTempo.Standby]);
        }
    }

    private void TimeChangeEnded(EnumTemporality temporality)
    {
        _changedTime = true;
        _character.CanChangeTime = true;
    }
}
