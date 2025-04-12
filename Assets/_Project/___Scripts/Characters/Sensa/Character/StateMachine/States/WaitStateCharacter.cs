using UnityEngine;

public class WaitStateCharacter : BaseStateCharacter<EnumStateCharacter>
{
    /// <summary>
    /// State ou le joueur aura les inputs bloqués pour les cinématiques
    /// pourra surement prendre en paramètre un sequencer, et jouera ce sequncer dans ce state
    /// </summary>

    new private ACharacter _character;

    float _clockZoom;

    float _startZoom = 30;
    float _endZoom = 20;

    public override void InitState(StateMachinePawn<EnumStateCharacter, BaseStatePawn<EnumStateCharacter>> stateMachine, EnumStateCharacter enumValue, APawn<EnumStateCharacter> character)
    {
        base.InitState(stateMachine, enumValue, character);
        _character = (ACharacter)character;
    }

    public override void EnterState()
    {
        base.EnterState();

        GameManager.Instance.CameraHandler.OnZoomCamera(_startZoom, _endZoom);


        _character.InputManager.OnInteract += OnInteract;
    }

    public override void ExitState()
    {
        base.ExitState();

        GameManager.Instance.CameraHandler.OnZoomCamera(_endZoom, _startZoom);

        _character.InputManager.OnInteract -= OnInteract;
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }

    public override void CheckChangeState()
    {
        base.CheckChangeState();

        if (_character.IsChangingTime)
        {
            _stateMachine.ChangeState(_stateMachine.States[EnumStateCharacter.ChangeTempo]);
        }

        else if (_character.InputManager.GetMoveDirection() != Vector2.zero)
        {
            _stateMachine.ChangeState(_stateMachine.States[EnumStateCharacter.Move]);
        }
    }

    private void OnInteract()
    {
        _stateMachine.ChangeState(_stateMachine.States[EnumStateCharacter.Interact]);
    }

  

}
