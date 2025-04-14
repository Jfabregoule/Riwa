using System;
using System.Collections.Generic;
using UnityEngine;

public class PawnCheckStateInteract<TStateEnum> : PawnInteractBaseSubstate<TStateEnum>
    where TStateEnum : Enum
{

    protected readonly List<GameObject> _colliderList = new();

    public override void InitState(PawnInteractSubstateMachine<TStateEnum> stateMachine, EnumInteract enumValue, APawn<TStateEnum> character)
    {
        base.InitState(stateMachine, enumValue, character);
    }

    public override void EnterState()
    {
        base.EnterState();


        _colliderList.Clear();

        float security = 1.5f;

        float heigth = _character.CapsuleCollider.height * _character.transform.localScale.x;

        Vector3 point1 = _character.transform.position + Vector3.down * heigth / 2 * security;
        Vector3 point2 = _character.transform.position + Vector3.up * heigth / 2 * security;
        float radius = heigth * security;

        //Offset pour mettre la capsule devant le joueur
        point1 += _character.transform.forward * radius * 1.5f;
        point2 += _character.transform.forward * radius * 1.5f;

        LayerMask layerMask = _character.IsInPast ? _character.PastLayer : _character.PresentLayer;

        Collider[] others = Physics.OverlapCapsule(point1, point2, radius, layerMask);

        foreach (Collider collider in others)
        {
            if (collider.gameObject.TryGetComponent<IInteractable>(out IInteractable obj))
            {
                _colliderList.Add(collider.gameObject);
            }
        }

        if (_colliderList.Count <= 0)
        {
            //Si il n'y a aucun obj interactaible, rien ne se passes
            ChangeStateToIdle();
            return;
        }

        _subStateMachine.CurrentObjectInteract = SortObjects(_character.transform.position, _colliderList);

        //Si le offset Radius est de -1 on ne move pas
        float radiusOffset = _subStateMachine.CurrentObjectInteract.GetComponent<IInteractable>().OffsetRadius;

        if (radiusOffset < 0)
        {
            _stateMachine.ChangeState(_stateMachine.States[EnumInteract.Action]);
            return;
        }

    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        CheckChangeState();
    }

    public override void CheckChangeState()
    {
        base.CheckChangeState();
        ChangeStateToMove();
    }

    public virtual void ChangeStateToIdle() { }
    public virtual void ChangeStateToMove() { }

}
