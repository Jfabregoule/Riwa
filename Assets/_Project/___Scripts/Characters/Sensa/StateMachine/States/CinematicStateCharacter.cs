using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicStateCharacter : BaseStateCharacter
{
    /// <summary>
    /// State ou le joueur aura les inputs bloqués pour les cinématiques
    /// pourra surement prendre en paramètre un sequencer, et jouera ce sequncer dans ce state
    /// </summary>

    public override void InitState(StateMachineCharacter stateMachine, EnumStateCharacter enumValue, ACharacter character)
    {
        base.InitState(stateMachine, enumValue, character);
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

    public override void CheckChangeState()
    {
        base.CheckChangeState();
    }
}
