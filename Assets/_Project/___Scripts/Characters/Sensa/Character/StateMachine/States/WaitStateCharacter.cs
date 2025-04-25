using System;
using UnityEngine;

public class WaitStateCharacter : BaseStateCharacter<EnumStateCharacter>
{
    /// <summary>
    /// State ou le joueur aura les inputs bloqués pour les cinématiques
    /// pourra surement prendre en paramètre un sequencer, et jouera ce sequncer dans ce state
    /// </summary>
    
    float _clockZoom;

    float _startZoom = 30;
    float _endZoom = 20;

    public override void InitState(StateMachinePawn<EnumStateCharacter, BaseStatePawn<EnumStateCharacter>> stateMachine, EnumStateCharacter enumValue, APawn<EnumStateCharacter> character)
    {
        base.InitState(stateMachine, enumValue, character);
    }

    public override void EnterState()
    {
        base.EnterState();

        GameManager.Instance.CameraHandler.OnZoomCamera(_startZoom, _endZoom);
        ACharacter chara = (ACharacter)_character;
        chara.InputManager.OnChangeTime += ChangeStateToTempo;
        _character.InputManager.OnInteract += OnInteract;
    }

    public override void ExitState()
    {
        base.ExitState();

        GameManager.Instance.CameraHandler.OnZoomCamera(_endZoom, _startZoom);
        ACharacter chara = (ACharacter)_character;
        chara.InputManager.OnChangeTime -= ChangeStateToTempo;
        _character.InputManager.OnInteract -= OnInteract;
    }

    public override void DestroyState()
    {
        base.DestroyState();

        ACharacter chara = (ACharacter)_character;
        chara.InputManager.OnChangeTime -= ChangeStateToTempo;
        _character.InputManager.OnInteract -= OnInteract;
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }

    public override void CheckChangeState()
    {
        base.CheckChangeState();

        ACharacter chara = (ACharacter)_character;

        if (chara.IsChangingTime)
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

    public void ChangeStateToTempo()
    {
        if (((ACharacter)_character).CanChangeTime)
        {
            _stateMachine.ChangeState(_stateMachine.States[EnumStateCharacter.ChangeTempo]);
        }
    }



}
