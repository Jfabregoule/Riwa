using UnityEngine;

public class HoldingStateCharacter : BaseStateCharacter<EnumStateCharacter>
{
    /// <summary>
    /// State dans lequel Sensa va tenir un gros objet
    /// 
    /// </summary>

    private HoldingStateMachine _subStateMachine;

    public override void InitState(StateMachinePawn<EnumStateCharacter, BaseStatePawn<EnumStateCharacter>> stateMachine, EnumStateCharacter enumValue, APawn<EnumStateCharacter> character)
    {
        base.InitState(stateMachine, enumValue, character);

        _subStateMachine = new HoldingStateMachine();
        _subStateMachine.InitStateMachine((ACharacter)character);
        _subStateMachine.InitState(_subStateMachine.States[EnumHolding.StandBy]);
        _character.Animator.ResetTrigger(_subStateMachine.AnimationMap[EnumHolding.IdleHolding]);
    }

    public override void EnterState()
    {
        base.EnterState();

        _subStateMachine.ChangeState(_subStateMachine.States[EnumHolding.IdleHolding]);
        _character.InputManager.OnInteract += OnInteractEnd;
        
        ACharacter character = (ACharacter)_character;
        if (character.HoldingObject.TryGetComponent(out IRotatable rotatable))
            GameManager.Instance.UIManager.Display(UIElementEnum.Rotate);
        if (character.HoldingObject.TryGetComponent(out IMovable movable))
        {
            GameManager.Instance.UIManager.Display(UIElementEnum.Push);
            GameManager.Instance.UIManager.Display(UIElementEnum.Pull);
        }
        GameManager.Instance.UIManager.Hide(UIElementEnum.ChangeTime);
        _character.InputManager.DisableGameplayMoveControls();
    }

    public override void ExitState()
    {
        base.ExitState();

        ACharacter chara = (ACharacter)_character;

        if (chara.HoldingObject.TryGetComponent(out TemporalItem temporalItem))
        {
            temporalItem.UpdatePresentPosition();
        }

        chara.SetHoldingObject(null);
        chara.InputManager.OnInteract -= OnInteractEnd;
        _subStateMachine.ChangeState(_subStateMachine.States[EnumHolding.StandBy]);
        GameManager.Instance.UIManager.Hide(UIElementEnum.Push);
        GameManager.Instance.UIManager.Hide(UIElementEnum.Pull);
        GameManager.Instance.UIManager.Hide(UIElementEnum.Rotate);
        GameManager.Instance.UIManager.Display(UIElementEnum.ChangeTime);
        _character.InputManager.EnableGameplayMoveControls();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        _subStateMachine.StateMachineUpdate();
    }

    public override void CheckChangeState()
    {
        base.CheckChangeState();

        //A definir si on maintient appuye ou si on toggle pour retourner en idle
    }

    private void OnInteractEnd()
    {
        _stateMachine.ChangeState(_stateMachine.States[EnumStateCharacter.Idle]);
    }

    public override void DestroyState()
    {
        _character.InputManager.OnInteract -= OnInteractEnd;
    }
}
