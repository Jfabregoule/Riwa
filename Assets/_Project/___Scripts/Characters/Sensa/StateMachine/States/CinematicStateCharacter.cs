using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicStateCharacter : BaseStateCharacter
{
    /// <summary>
    /// State ou le joueur aura les inputs bloqués pour les cinématiques
    /// pourra surement prendre en paramètre un sequencer, et jouera ce sequncer dans ce state
    /// </summary>

    public override void InitState(StateMachineCharacter stateMachine, Character character)
    {
        base.InitState(stateMachine, character);

        _enumState = EnumStateCharacter.Cinematic;
    }

    public override void EnterState()
    {
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void UpdateState(float dT)
    {
        base.UpdateState(dT);
    }

    public override void ChangeState()
    {
        base.ChangeState();
    }
}
