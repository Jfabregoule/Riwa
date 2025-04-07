using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineSoul : BaseStateMachine<EnumStateSoul, BaseStateSoul>
{

    #region StateNameAnimation

    private const string IDLE_NAME          = "Idle";
    private const string MOVE_NAME          = "Move";

    #endregion

    public StateMachineSoul() { 

        _transition = new BaseTransitions();
        States = new Dictionary<EnumStateSoul, BaseStateSoul>();
        _animationMap = new Dictionary<EnumStateSoul, string>();
    }

    public void InitStateMachine(ASoul Soul)
    {
        States[EnumStateSoul.Idle] = new IdleStateSoul();
        States[EnumStateSoul.Idle].InitState(this, EnumStateSoul.Idle, Soul);
        _animationMap[EnumStateSoul.Idle] = IDLE_NAME;

        States[EnumStateSoul.Move] = new MoveStateSoul();
        States[EnumStateSoul.Move].InitState(this, EnumStateSoul.Move, Soul);
        _animationMap[EnumStateSoul.Move] = MOVE_NAME;
    }

}
