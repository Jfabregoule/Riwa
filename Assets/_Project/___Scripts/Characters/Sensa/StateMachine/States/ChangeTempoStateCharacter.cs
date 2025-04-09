using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTempoStateCharacter : BaseStateCharacter<EnumStateCharacter>
{
    /// <summary>
    /// State dans lequel le joueur va changer de temporalité
    /// L'entrée vers ce state n'a pas encore été définit
    /// On y fera le check de si le voyage temporel est possible ou non
    /// </summary>

    private ChangeTempoStateMachine _subStateMachine;

    public override void InitState(StateMachineCharacter stateMachine, EnumStateCharacter enumValue, ACharacter character)
    {
        base.InitState(stateMachine, enumValue, character);

        ///////////////////

        _subStateMachine = new ChangeTempoStateMachine();
        _subStateMachine.InitStateMachine(_character);
        _subStateMachine.InitState(_subStateMachine.States[EnumChangeTempo.Standby]);
    }

    public override void EnterState()
    {
        base.EnterState();

        _subStateMachine.ChangeState(_subStateMachine.States[EnumChangeTempo.Check]);

    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        _subStateMachine.StateMachineUpdate();
    }

    public override void CheckChangeState()
    {
        base.CheckChangeState();

        if (_character.IsChangingTime == false)
        {
            _stateMachine.ChangeState(_stateMachine.States[EnumStateCharacter.Idle]);
            return;
        }
    }
}
