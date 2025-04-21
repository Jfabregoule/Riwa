using UnityEngine;

public class CharacterMoveStateInteract : PawnMoveStateInteract<EnumStateCharacter>
{
    public override void InitState(PawnInteractSubstateMachine<EnumStateCharacter> stateMachine, EnumInteract enumValue, APawn<EnumStateCharacter> character)
    {
        base.InitState(stateMachine, enumValue, character);
    }

    public override void EnterState()
    {
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }

    public override void CheckChangeState()
    {
        base.CheckChangeState();
    }

    public override void InteractEndOfPath()
    {
        //Ce qu'il se passe a la fin du move dans character

        if (_subStateMachine.CurrentObjectInteract.TryGetComponent(out IHoldable holdable))
        {
            if (_endInteract)
            {
                ChangeStateToIdle();
            }
            else
            {
                SetHoldingObject();
            }
            return;
        }

        foreach (var comp in _subStateMachine.CurrentObjectInteract.GetComponents<MonoBehaviour>())
        {
            if (comp is ITreeStump stump && comp.enabled)
            {
                foreach (var comp2 in _subStateMachine.CurrentObjectInteract.GetComponents<MonoBehaviour>())
                {
                    if (comp2 is IInteractable interactable && comp2.enabled)
                    {
                        interactable.Interact();
                        break;
                    }
                }

                ChangeStateToSoul();
                return;
            }
        }

        //Default
        _subStateMachine.ChangeState(_subStateMachine.States[EnumInteract.Action]);

    }

    protected override void ChangeStateToIdle()
    {
        ACharacter chara = (ACharacter)_character;
        chara.StateMachine.ChangeState(chara.StateMachine.States[EnumStateCharacter.Idle]);
    }
    protected override void ChangeStateToSoul()
    {
        ACharacter chara = (ACharacter)_character;
        chara.StateMachine.ChangeState(chara.StateMachine.States[EnumStateCharacter.Soul]);
    }
    protected override void SetHoldingObject()
    {
        ACharacter charac = (ACharacter)_character;
        charac.SetHoldingObject(_subStateMachine.CurrentObjectInteract);
        charac.StateMachine.ChangeState(charac.StateMachine.States[EnumStateCharacter.Holding]);
    }

}
