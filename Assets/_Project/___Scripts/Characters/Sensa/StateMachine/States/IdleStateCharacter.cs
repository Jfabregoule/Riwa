using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleStateCharacter : BaseStateCharacter
{
    private float _clock;

    public override void InitState(StateMachineCharacter stateMachine, EnumStateCharacter enumValue, ACharacter character)
    {
        base.InitState(stateMachine, enumValue, character);
    }

    public override void EnterState()
    {
        base.EnterState();

        _character.InputManager.OnInteract += OnInteract;

        _clock = 0;
    }

    public override void ExitState()
    {
        base.ExitState();

        _character.InputManager.OnInteract -= OnInteract;
    }

    public override void UpdateState(float dT)
    {
        base.UpdateState(dT);

        _clock += dT;
    }

    public override void CheckChangeState()
    {
        base.CheckChangeState();

        if (_character.IsChangingTime)
        {
            _stateMachine.ChangeState(_stateMachine.States[EnumStateCharacter.ChangeTempo]);
        }

        if (_character.InputManager.GetMoveDirection() != Vector2.zero)
        {
            _stateMachine.ChangeState(_stateMachine.States[EnumStateCharacter.Move]);
            return;
        }

        if (_clock > _character.TimeBeforeWait)
        {
            //_stateMachine.ChangeState(_stateMachine.States[EnumStateCharacter.Wait]);
            return;
        }
    }

    private void OnInteract()
    {
        _stateMachine.ChangeState(_stateMachine.States[EnumStateCharacter.Interact]);
    }
}
