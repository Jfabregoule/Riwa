using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulStateCharacter : BaseStateCharacter
{
    /// <summary>
    /// State dans lequel le joueur va changer de temporalité
    /// L'entrée vers ce state n'a pas encore été définit
    /// On y fera le check de si le voyage temporel est possible ou non
    /// </summary>

    private SoulStateMachine _subStateMachine;

    public override void InitState(StateMachineCharacter stateMachine, EnumStateCharacter enumValue, ACharacter character)
    {
        base.InitState(stateMachine, enumValue, character);

        ///////////////////

        _subStateMachine = new SoulStateMachine();
        _subStateMachine.InitStateMachine(_character);
        _subStateMachine.InitState(_subStateMachine.States[EnumSoul.IdleSoul]);
    }

    public override void EnterState()
    {
        base.EnterState();

        _subStateMachine.ChangeState(_subStateMachine.States[EnumSoul.IdleSoul]);

    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void UpdateState(float dT)
    {
        base.UpdateState(dT);

        _subStateMachine.StateMachineUpdate(dT);
    }

    public override void CheckChangeState()
    {
        base.CheckChangeState();

        if (_character.IsInSoul == false)
        {
            _stateMachine.ChangeState(_stateMachine.States[EnumStateCharacter.Idle]);
            return;
        }
    }
}
