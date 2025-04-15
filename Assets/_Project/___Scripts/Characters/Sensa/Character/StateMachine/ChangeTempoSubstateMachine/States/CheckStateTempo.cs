using UnityEngine;

public class CheckStateTempo : ChangeTempoBaseState
{
    private EnumChangeTempo _nextState = EnumChangeTempo.Standby;
    private Vector3 _point1, _point2;
    private float _radius;

    public override void InitState(ChangeTempoStateMachine stateMachine, EnumChangeTempo enumValue, ACharacter character)
    {
        base.InitState(stateMachine, enumValue, character);
    }

    public override void EnterState()
    {
        base.EnterState();

        float security = 0.95f;

        _point1 = _character.transform.position + Vector3.down * _character.CapsuleCollider.height / 2 * security;
        _point2 = _character.transform.position + Vector3.up * _character.CapsuleCollider.height / 2 * security;
        _radius = _character.CapsuleCollider.radius * security;

        LayerMask layerMask = _character.IsInPast ? _character.PresentLayer : _character.PastLayer;

        if (Physics.CheckCapsule(_point1, _point2, _radius, layerMask))
        {
            _nextState = EnumChangeTempo.Cancel;
        }
        else
        {
            _nextState = EnumChangeTempo.Process;
        }
    }

    public override void ExitState()
    {
        base.ExitState();
        _nextState = EnumChangeTempo.Standby;

        _character.StateMachine.ChangeState(_character.StateMachine.States[EnumStateCharacter.Idle]);
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }

    public override void CheckChangeState()
    {
        base.CheckChangeState();

        if (_nextState != EnumChangeTempo.Standby)
        {
            _stateMachine.ChangeState(_stateMachine.States[_nextState]);
        }
    }
}
