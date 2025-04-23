using UnityEngine;

public class IdleStateCharacter : ParentIdleState<EnumStateCharacter>
{
    public override void InitState(StateMachinePawn<EnumStateCharacter, BaseStatePawn<EnumStateCharacter>> stateMachine, EnumStateCharacter enumValue, APawn<EnumStateCharacter> character)
    {
        base.InitState(stateMachine, enumValue, character);

    }

    public override void EnterState()
    {
        base.EnterState();

        ACharacter chara = (ACharacter)_character;

        chara.InputManager.OnInteract += OnInteract;
        chara.InputManager.OnChangeTime += ChangeStateToTempo;

        _clock = 0;

        chara.Feet.OnFall += GoToFall;

    }

    public override void ExitState()
    {
        base.ExitState();

        ACharacter chara = (ACharacter)_character;

        chara.InputManager.OnInteract -= OnInteract;
        chara.InputManager.OnChangeTime -= ChangeStateToTempo;

        chara.Feet.OnFall -= GoToFall;
    }

    public override void UpdateState()
    {
        base.UpdateState();

        _clock += Time.deltaTime;

    }

    public override void CheckChangeState()
    {
        base.CheckChangeState();

        ACharacter chara = (ACharacter)_character;

        if (_character.InputManager.GetMoveDirection() != Vector2.zero)
        {
            _stateMachine.ChangeState(_stateMachine.States[EnumStateCharacter.Move]);
            return;
        }

        else if (_clock > chara.TimeBeforeWait)
        {
            _stateMachine.ChangeState(_stateMachine.States[EnumStateCharacter.Wait]);
            return;
        }
    }

    private void OnInteract()
    {
        _stateMachine.ChangeState(_stateMachine.States[EnumStateCharacter.Interact]);
    }

    public void ChangeStateToTempo()
    {
        if (((ACharacter)_character).CanChangeTime)
        {
            _stateMachine.ChangeState(_stateMachine.States[EnumStateCharacter.ChangeTempo]);
        }
    }

    private void GoToFall() 
    { 
        _stateMachine.ChangeState(_stateMachine.States[EnumStateCharacter.Fall]);
    
    }

}
