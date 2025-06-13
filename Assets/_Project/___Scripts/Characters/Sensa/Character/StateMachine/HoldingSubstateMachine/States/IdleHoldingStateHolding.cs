using UnityEngine;

public class IdleHoldingStateHolding : HoldingBaseState
{
    public override void InitState(HoldingStateMachine stateMachine, EnumHolding enumValue, ACharacter character)
    {
        base.InitState(stateMachine, enumValue, character);
    }

    public override void EnterState()
    {
        base.EnterState();
        _character.InputManager.OnRotateLeft += OnRotateLeft;
        _character.InputManager.OnRotateRight += OnRotateRight;
        _character.InputManager.OnPush += OnPush;
        _character.InputManager.OnPull += OnPull;
    }

    public override void ExitState()
    {
        base.ExitState();
        _character.InputManager.OnRotateLeft -= OnRotateLeft;
        _character.InputManager.OnRotateRight -= OnRotateRight;
        _character.InputManager.OnPush -= OnPush;
        _character.InputManager.OnPull -= OnPull;
    }

    public override void UpdateState()
    {
        base.UpdateState();
        
    }

    public override void CheckChangeState()
    {
        base.CheckChangeState();
    }

    public override void DestroyState()
    {
        base.DestroyState();
        _character.InputManager.OnRotateLeft -= OnRotateLeft;
        _character.InputManager.OnRotateRight -= OnRotateRight;
        _character.InputManager.OnPush -= OnPush;
        _character.InputManager.OnPull -= OnPull;
    }

    private void OnRotateLeft()
    {
        ((RotateStateHolding)_stateMachine.States[EnumHolding.Rotate]).Sens = 1;
        _stateMachine.ChangeState(_stateMachine.States[EnumHolding.Rotate]);
    }

    private void OnRotateRight()
    {
        ((RotateStateHolding)_stateMachine.States[EnumHolding.Rotate]).Sens = -1;
        _stateMachine.ChangeState(_stateMachine.States[EnumHolding.Rotate]);
    }

    private void OnPush()
    {
        ((MoveStateHolding)_stateMachine.States[EnumHolding.Move]).Sens = 1;
        _stateMachine.ChangeState(_stateMachine.States[EnumHolding.Move]);
    }

    private void OnPull()
    {
        ((MoveStateHolding)_stateMachine.States[EnumHolding.Move]).Sens = -1;
        _stateMachine.ChangeState(_stateMachine.States[EnumHolding.Move]);
    }
}
