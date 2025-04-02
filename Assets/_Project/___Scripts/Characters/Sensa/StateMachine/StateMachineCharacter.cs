using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineCharacter : BaseStateMachine<EnumStateCharacter, BaseStateCharacter>
{

    #region StateNameAnimation

    private const string IDLE_NAME          = "Idle";
    private const string MOVE_NAME          = "Move";
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
        States = new Dictionary<EnumStateCharacter, BaseStateCharacter>();
        _animationMap = new Dictionary<EnumStateCharacter, string>();
    }

    public void InitStateMachine(ACharacter character)
    {
        States[EnumStateCharacter.Idle] = new IdleStateCharacter();
        States[EnumStateCharacter.Idle].InitState(this, EnumStateCharacter.Idle, character);
        _animationMap[EnumStateCharacter.Idle] = IDLE_NAME;

        States[EnumStateCharacter.Move] = new MoveStateCharacter();
        States[EnumStateCharacter.Move].InitState(this, EnumStateCharacter.Move, character);
        _animationMap[EnumStateCharacter.Move] = MOVE_NAME;

        States[EnumStateCharacter.Interact] = new InteractStateCharacter();
        States[EnumStateCharacter.Interact].InitState(this, EnumStateCharacter.Interact, character);
        _animationMap[EnumStateCharacter.Interact] = INTERACT_NAME;

        States[EnumStateCharacter.ChangeTempo] = new ChangeTempoStateCharacter();
        States[EnumStateCharacter.ChangeTempo].InitState(this, EnumStateCharacter.ChangeTempo, character);
        _animationMap[EnumStateCharacter.ChangeTempo] = CHANGETEMPO_NAME;

        States[EnumStateCharacter.Holding] = new HoldingStateCharacter();
        States[EnumStateCharacter.Holding].InitState(this, EnumStateCharacter.Holding, character);
        _animationMap[EnumStateCharacter.Holding] = HOLDING_NAME;

        States[EnumStateCharacter.Pull] = new PullStateCharacter();
        States[EnumStateCharacter.Pull].InitState(this, EnumStateCharacter.Pull, character);
        _animationMap[EnumStateCharacter.Pull] = PULL_NAME;

        States[EnumStateCharacter.Push] = new PushStateCharacter();
        States[EnumStateCharacter.Push].InitState(this, EnumStateCharacter.Push, character);
        _animationMap[EnumStateCharacter.Push] = PUSH_NAME;

        States[EnumStateCharacter.SoulIdle] = new SoulIdleStateCharacter();
        States[EnumStateCharacter.SoulIdle].InitState(this, EnumStateCharacter.SoulIdle, character);
        _animationMap[EnumStateCharacter.SoulIdle] = SOULIDLE_NAME;

        States[EnumStateCharacter.SoulWalk] = new SoulWalkStateCharacter();
        States[EnumStateCharacter.SoulWalk].InitState(this, EnumStateCharacter.SoulWalk, character);
        _animationMap[EnumStateCharacter.SoulWalk] = SOULWALK_NAME;

        States[EnumStateCharacter.Cinematic] = new CinematicStateCharacter();
        States[EnumStateCharacter.Cinematic].InitState(this, EnumStateCharacter.Cinematic, character);
        _animationMap[EnumStateCharacter.Cinematic] = CINEMATIC_NAME;

        States[EnumStateCharacter.Wait] = new WaitStateCharacter();
        States[EnumStateCharacter.Wait].InitState(this, EnumStateCharacter.Wait, character);
        _animationMap[EnumStateCharacter.Wait] = WAIT_NAME;

        States[EnumStateCharacter.Rotate] = new RotateStateCharacter();
        States[EnumStateCharacter.Rotate].InitState(this, EnumStateCharacter.Rotate, character);
        _animationMap[EnumStateCharacter.Rotate] = ROTATE_NAME;

        States[EnumStateCharacter.Fall] = new FallStateCharacter();
        States[EnumStateCharacter.Fall].InitState(this, EnumStateCharacter.Fall, character);
        _animationMap[EnumStateCharacter.Fall] = FALL_NAME;

        States[EnumStateCharacter.Respawn] = new RespawnStateCharacter();
        States[EnumStateCharacter.Respawn].InitState(this, EnumStateCharacter.Respawn, character);
        _animationMap[EnumStateCharacter.Respawn] = RESPAWN_NAME;

    }

}
