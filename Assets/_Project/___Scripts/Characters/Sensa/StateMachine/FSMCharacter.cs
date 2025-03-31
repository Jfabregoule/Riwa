using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMCharacter
{
    private Dictionary<EnumStateCharacter, BaseStateCharacter> _states; //Liste des states

    private BaseStateCharacter                                  _currentState;
    private Transitions                                         _transition; //Class qui va contenir toutes les transitions 
    private Dictionary<EnumStateCharacter, string>              _animationMap; //Dicionnaire pour trigger la bonne animation pour chaque state

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

    #endregion

    public BaseStateCharacter CurrentState { get => _currentState; }
    public Transitions Transition { get => _transition;}
    public Dictionary<EnumStateCharacter, string> AnimationMap { get => _animationMap; }
    public Dictionary<EnumStateCharacter, BaseStateCharacter> States { get => _states; }

    public FSMCharacter() { 
        _transition = new Transitions();
        _states = new Dictionary<EnumStateCharacter, BaseStateCharacter>();
        _animationMap = new Dictionary<EnumStateCharacter, string>();
    }

    public void InitStateMachine(Character character)
    {
        _states[EnumStateCharacter.Idle] = new IdleStateCharacter();
        _states[EnumStateCharacter.Idle].InitState(this, character);
        _animationMap[EnumStateCharacter.Idle] = IDLE_NAME;

        _states[EnumStateCharacter.Walk] = new WalkStateCharacter();
        _states[EnumStateCharacter.Walk].InitState(this, character);
        _animationMap[EnumStateCharacter.Walk] = WALK_NAME;

        _states[EnumStateCharacter.Run] = new RunStateCharacter();
        _states[EnumStateCharacter.Run].InitState(this, character);
        _animationMap[EnumStateCharacter.Run] = RUN_NAME;

        _states[EnumStateCharacter.Interact] = new InteractStateCharacter();
        _states[EnumStateCharacter.Interact].InitState(this, character);
        _animationMap[EnumStateCharacter.Interact] = INTERACT_NAME;

        _states[EnumStateCharacter.ChangeTempo] = new ChangeTempoStateCharacter();
        _states[EnumStateCharacter.ChangeTempo].InitState(this, character);
        _animationMap[EnumStateCharacter.ChangeTempo] = CHANGETEMPO_NAME;

        _states[EnumStateCharacter.Holding] = new HoldingStateCharacter();
        _states[EnumStateCharacter.Holding].InitState(this, character);
        _animationMap[EnumStateCharacter.Holding] = HOLDING_NAME;

        _states[EnumStateCharacter.Pull] = new PullStateCharacter();
        _states[EnumStateCharacter.Pull].InitState(this, character);
        _animationMap[EnumStateCharacter.Pull] = PULL_NAME;

        _states[EnumStateCharacter.Push] = new PushStateCharacter();
        _states[EnumStateCharacter.Push].InitState(this, character);
        _animationMap[EnumStateCharacter.Push] = PUSH_NAME;

        _states[EnumStateCharacter.SoulIdle] = new SoulIdleStateCharacter();
        _states[EnumStateCharacter.SoulIdle].InitState(this, character);
        _animationMap[EnumStateCharacter.SoulIdle] = SOULIDLE_NAME;

        _states[EnumStateCharacter.SoulWalk] = new SoulWalkStateCharacter();
        _states[EnumStateCharacter.SoulWalk].InitState(this, character);
        _animationMap[EnumStateCharacter.SoulWalk] = SOULWALK_NAME;

        _states[EnumStateCharacter.Cinematic] = new CinematicCharacterState();
        _states[EnumStateCharacter.Cinematic].InitState(this, character);
        _animationMap[EnumStateCharacter.Cinematic] = CINEMATIC_NAME;

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

    public void StateMachineUpdate()
    {
        _currentState.UpdateState();
    }

}
