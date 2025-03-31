using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicCharacterState : BaseStateCharacter
{
    /// <summary>
    /// State ou le joueur aura les inputs bloqu�s pour les cin�matiques
    /// pourra surement prendre en param�tre un sequencer, et jouera ce sequncer dans ce state
    /// </summary>

    public override void InitState(FSMCharacter stateMachine, Character character)
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

    public override void UpdateState()
    {
        base.UpdateState();
    }

    public override void ChangeState()
    {
        base.ChangeState();
    }
}
