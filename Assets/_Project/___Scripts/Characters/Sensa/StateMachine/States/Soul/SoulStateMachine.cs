using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulStateMachine : BaseStateMachine<EnumSoul, SoulBaseState>
{
    private const string IDLESOUL_NAME = "IdleSoul";
    private const string MOVESOUL_NAME = "MoveSoul";

    public SoulStateMachine()
    {
        //animation map et transition pas obligatoires

        _transition = new BaseTransitions(); //Dans l'idéal créer une class transition par state machine 
        States = new Dictionary<EnumSoul, SoulBaseState>();
        _animationMap = new Dictionary<EnumSoul, string>();
    }

    public void InitStateMachine(ACharacter character)
    {
        States[EnumSoul.IdleSoul] = new IdleSoulStateSoul();
        States[EnumSoul.IdleSoul].InitState(this, EnumSoul.IdleSoul, character);
        _animationMap[EnumSoul.IdleSoul] = IDLESOUL_NAME;

        States[EnumSoul.MoveSoul] = new MoveStateSoul();
        States[EnumSoul.MoveSoul].InitState(this, EnumSoul.MoveSoul, character);
        _animationMap[EnumSoul.MoveSoul] = MOVESOUL_NAME;
    }
}
