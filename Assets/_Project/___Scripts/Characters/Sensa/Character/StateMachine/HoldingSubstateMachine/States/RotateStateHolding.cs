
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class RotateStateHolding : HoldingBaseState
{
    private int _sens;

    private float _clock;

    private IRotatable _rotatable;
    public int Sens { get => _sens; set => _sens = value; }
    public override void InitState(HoldingStateMachine stateMachine, EnumHolding enumValue, ACharacter character)
    {
        base.InitState(stateMachine, enumValue, character);
    }

    public override void EnterState()
    {
        base.EnterState();

        if (_character.HoldingObject.TryGetComponent(out IRotatable rotatable))
        {
            _rotatable = rotatable;
        }

        _character.Animator.SetFloat("HoldingSens", Sens);
        _character.Animator.SetFloat("RotationSpeed", _rotatable.RotateSpeed);

        _character.OnRotate += LaunchRotate;
    }

    private void LaunchRotate()
    {
        if (_character.HoldingObject.TryGetComponent(out IRotatable rotatable))
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
        _character.OnRotate -= LaunchRotate;

        _character.InputManager.OnRotateLeft -= OnRotateLeft;
        _character.InputManager.OnRotateRight -= OnRotateRight;

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
        Vector2 joystickDir = _character.InputManager.GetMoveDirection();

        if (joystickDir == Vector2.zero)
        {
            _stateMachine.ChangeState(_stateMachine.States[EnumHolding.IdleHolding]);
            return;
        }

        _character.InputManager.OnRotateLeft += OnRotateLeft;
        _character.InputManager.OnRotateRight += OnRotateRight;
    }

    public override void DestroyState()
    {
        base.DestroyState();

        _character.OnRotate -= LaunchRotate;
        _rotatable.OnRotateFinished -= Finish;

        _character.InputManager.OnRotateLeft -= OnRotateLeft;
        _character.InputManager.OnRotateRight -= OnRotateRight;
    }

    private void OnRotateLeft()
    {
        if (!_character.HoldingObject.TryGetComponent(out IRotatable rotatable) && Sens == 1) return;
        _stateMachine.ChangeState(_stateMachine.States[EnumHolding.Rotate]);
    }

    private void OnRotateRight()
    {
        if (!_character.HoldingObject.TryGetComponent(out IRotatable rotatable) && Sens == -1) return;
        _stateMachine.ChangeState(_stateMachine.States[EnumHolding.Rotate]);
    }

}
