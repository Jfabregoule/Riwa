using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineCharacter : BaseStateMachine<EnumStateCharacter>
{
    new protected Dictionary<EnumStateCharacter, BaseStateCharacter> _states; //Liste des states

    #region StateNameAnimation

    private const string IDLE_NAME          = "Idle";
    private const string WALK_NAME          = "Walk";
    private const string RUN_NAME           = "Run";
    private const string INTERACT_NAME      = "Interact";
    private const string CHANGETEMPO_NAME   = "ChangeTempo";
    private const string CINEMATIC_NAME     = "Cinematic";
    private const string HOLDING_NAME       = "Holding";
    private const string PUSH_NAME          = "Push";
    private const string PULL_NAME          = "Pull";
    private const string SOULIDLE_NAME      = "SoulIdle";
    private const string SOULWALK_NAME      = "SoulWalk";
    private const string WAIT_NAME          = "Wait";
    private const string ROTATE_NAME        = "Rotate";
    private const string FALL_NAME          = "Fall";
    private const string RESPAWN_NAME       = "Respawn";

    #endregion

    public StateMachineCharacter() { 

        _transition = new BaseTransitions();
        _states = new Dictionary<EnumStateCharacter, BaseStateCharacter>();
        _animationMap = new Dictionary<EnumStateCharacter, string>();
    }

    public void InitStateMachine(Character character)
    {
        _states[EnumStateCharacter.Idle] = new IdleStateCharacter();
        _states[EnumStateCharacter.Idle].InitState(this, EnumStateCharacter.Idle, character);
        _animationMap[EnumStateCharacter.Idle] = IDLE_NAME;

        _states[EnumStateCharacter.Walk] = new WalkStateCharacter();
        _states[EnumStateCharacter.Walk].InitState(this, EnumStateCharacter.Walk, character);
        _animationMap[EnumStateCharacter.Walk] = WALK_NAME;

        _states[EnumStateCharacter.Run] = new RunStateCharacter();
        _states[EnumStateCharacter.Run].InitState(this, EnumStateCharacter.Run, character);
        _animationMap[EnumStateCharacter.Run] = RUN_NAME;

        _states[EnumStateCharacter.Interact] = new InteractStateCharacter();
        _states[EnumStateCharacter.Interact].InitState(this, EnumStateCharacter.Interact, character);
        _animationMap[EnumStateCharacter.Interact] = INTERACT_NAME;

        _states[EnumStateCharacter.ChangeTempo] = new ChangeTempoStateCharacter();
        _states[EnumStateCharacter.ChangeTempo].InitState(this, EnumStateCharacter.ChangeTempo, character);
        _animationMap[EnumStateCharacter.ChangeTempo] = CHANGETEMPO_NAME;

        _states[EnumStateCharacter.Holding] = new HoldingStateCharacter();
        _states[EnumStateCharacter.Holding].InitState(this, EnumStateCharacter.Holding, character);
        _animationMap[EnumStateCharacter.Holding] = HOLDING_NAME;

        _states[EnumStateCharacter.Pull] = new PullStateCharacter();
        _states[EnumStateCharacter.Pull].InitState(this, EnumStateCharacter.Pull, character);
        _animationMap[EnumStateCharacter.Pull] = PULL_NAME;

        _states[EnumStateCharacter.Push] = new PushStateCharacter();
        _states[EnumStateCharacter.Push].InitState(this, EnumStateCharacter.Push, character);
        _animationMap[EnumStateCharacter.Push] = PUSH_NAME;

        _states[EnumStateCharacter.SoulIdle] = new SoulIdleStateCharacter();
        _states[EnumStateCharacter.SoulIdle].InitState(this, EnumStateCharacter.SoulIdle, character);
        _animationMap[EnumStateCharacter.SoulIdle] = SOULIDLE_NAME;

        _states[EnumStateCharacter.SoulWalk] = new SoulWalkStateCharacter();
        _states[EnumStateCharacter.SoulWalk].InitState(this, EnumStateCharacter.SoulWalk, character);
        _animationMap[EnumStateCharacter.SoulWalk] = SOULWALK_NAME;

        _states[EnumStateCharacter.Cinematic] = new CinematicStateCharacter();
        _states[EnumStateCharacter.Cinematic].InitState(this, EnumStateCharacter.Cinematic, character);
        _animationMap[EnumStateCharacter.Cinematic] = CINEMATIC_NAME;

        _states[EnumStateCharacter.Wait] = new WaitStateCharacter();
        _states[EnumStateCharacter.Wait].InitState(this, EnumStateCharacter.Wait, character);
        _animationMap[EnumStateCharacter.Wait] = WAIT_NAME;

        _states[EnumStateCharacter.Rotate] = new RotateStateCharacter();
        _states[EnumStateCharacter.Rotate].InitState(this, EnumStateCharacter.Rotate, character);
        _animationMap[EnumStateCharacter.Rotate] = ROTATE_NAME;

        _states[EnumStateCharacter.Fall] = new FallStateCharacter();
        _states[EnumStateCharacter.Fall].InitState(this, EnumStateCharacter.Fall, character);
        _animationMap[EnumStateCharacter.Fall] = FALL_NAME;

        _states[EnumStateCharacter.Respawn] = new RespawnStateCharacter();
        _states[EnumStateCharacter.Respawn].InitState(this, EnumStateCharacter.Respawn, character);
        _animationMap[EnumStateCharacter.Respawn] = RESPAWN_NAME;

    }

    public void InitState(BaseStateCharacter initState)
    {
        _currentState = initState;
        _currentState.EnterState();
    }

    public void ChangeState(BaseStateCharacter newState)
    {
        //A refactor ça prend ptet trop de ressources
        if (_currentState.TransitionMap.ContainsKey(newState.EnumState))
        {
            _currentState.TransitionMap[newState.EnumState]?.Invoke();
        }
        _currentState.ExitState();
        _currentState = newState;
        _currentState.EnterState();


    }

    new public void StateMachineUpdate(float dT)
    {
        _currentState.UpdateState(dT);
    }

}
