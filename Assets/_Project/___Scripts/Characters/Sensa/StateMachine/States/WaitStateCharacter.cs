using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WaitStateCharacter : BaseStateCharacter
{
    /// <summary>
    /// State ou le joueur aura les inputs bloqués pour les cinématiques
    /// pourra surement prendre en paramètre un sequencer, et jouera ce sequncer dans ce state
    /// </summary>

    float _clockZoom;

    float _startZoom = 30;
    float _endZoom = 25;

    public override void InitState(StateMachineCharacter stateMachine, EnumStateCharacter enumValue, ACharacter character)
    {
        base.InitState(stateMachine, enumValue, character);
    }

    public override void EnterState()
    {
        base.EnterState();

        GameManager.Instance.CameraHandler.OnZoomCamera(_startZoom, _endZoom);

    }

    public override void ExitState()
    {
        base.ExitState();

        GameManager.Instance.CameraHandler.OnZoomCamera(_endZoom, _startZoom);
    }

    public override void UpdateState(float dT)
    {
        base.UpdateState(dT);
    }

    public override void CheckChangeState()
    {
        base.CheckChangeState();

        if (_character.Joystick.Direction.y != 0 || _character.Joystick.Direction.x != 0)
        {
            _stateMachine.ChangeState(_stateMachine.States[EnumStateCharacter.Move]);
        }
    }

  

}
