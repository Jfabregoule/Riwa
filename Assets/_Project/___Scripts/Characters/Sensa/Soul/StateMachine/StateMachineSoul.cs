using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineSoul : StateMachinePawn<EnumStateSoul, BaseStatePawn<EnumStateSoul>>
{

    #region StateNameAnimation

    private const string IDLE_NAME          = "Idle";
    private const string MOVE_NAME          = "Move";
    private const string INTERACT_NAME      = "Interact";

    #endregion

    public StateMachineSoul() { 

        _transition = new();
        States = new();
        _animationMap = new();
    }

    public void InitStateMachine(ASoul Soul)
    {
        base.InitStateMachine();

        States[EnumStateSoul.Idle] = new IdleStateSoul();
        States[EnumStateSoul.Idle].InitState(this, EnumStateSoul.Idle, Soul);
        _animationMap[EnumStateSoul.Idle] = IDLE_NAME;

        States[EnumStateSoul.Move] = new MoveStateSoul();
        States[EnumStateSoul.Move].InitState(this, EnumStateSoul.Move, Soul);
        _animationMap[EnumStateSoul.Move] = MOVE_NAME;

        States[EnumStateSoul.Interact] = new InteractStateSoul();
        States[EnumStateSoul.Interact].InitState(this, EnumStateSoul.Interact, Soul);
        _animationMap[EnumStateSoul.Interact] = INTERACT_NAME;
    }

    public override void GoToIdle()
    {
        ChangeState(States[EnumStateSoul.Idle]);
    }

    public override void GoToHolding()
    {
        return;
    }

    public override void GoToSoul()
    {
        //GameManager.Instance.Character.StateMachine.ChangeState();
        //Faire passer la soul en human
    }

}
