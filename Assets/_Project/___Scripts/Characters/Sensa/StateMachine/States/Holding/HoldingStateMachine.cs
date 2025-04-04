using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldingStateMachine : BaseStateMachine<EnumHolding, HoldingBaseState>
{
    private const string IDLEHOLDING_NAME = "IdleHolding";
    private const string PULL_NAME = "Pull";
    private const string PUSH_NAME = "Push";
    private const string ROTATE_NAME = "Rotate";

    public HoldingStateMachine()
    {
        //animation map et transition pas obligatoires

        _transition = new BaseTransitions(); //Dans l'idéal créer une class transition par state machine 
        States = new Dictionary<EnumHolding, HoldingBaseState>();
        _animationMap = new Dictionary<EnumHolding, string>();
    }

    public void InitStateMachine(ACharacter character)
    {
        
    }
}
