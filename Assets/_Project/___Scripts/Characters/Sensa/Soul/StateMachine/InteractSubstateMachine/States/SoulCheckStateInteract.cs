using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulCheckStateInteract : PawnCheckStateInteract<EnumStateSoul>
{
    new private ASoul _character;
    //new private SoulInteractSubstateMachine _stateMachine;

    private readonly List<GameObject> _colliderList = new();

    public override void InitState(PawnInteractSubstateMachine<EnumStateSoul> stateMachine, EnumInteract enumValue, APawn<EnumStateSoul> character)
    {
        base.InitState(stateMachine, enumValue, character);
        _character = (ASoul)character;
        //_stateMachine = (SoulInteractSubstateMachine)stateMachine

        if (_stateMachine.CurrentObjectInteract != null) { }

    }

    public override void EnterState()
    {
        base.EnterState();

        _colliderList.Clear();

        float security = 1.5f;

        Vector3 point1 = _character.transform.position + Vector3.down * _character.CapsuleCollider.height / 2 * security;
        Vector3 point2 = _character.transform.position + Vector3.up * _character.CapsuleCollider.height / 2 * security;
        float radius = _character.CapsuleCollider.radius * security;

        LayerMask layerMask = _character.IsInPast ? _character.PastLayer : _character.PresentLayer;

        //Offset pour mettre la capsule devant le joueur
        point1 += _character.transform.forward * radius * 1.5f;
        point2 += _character.transform.forward * radius * 1.5f;

        Collider[] others = Physics.OverlapCapsule(point1, point2, radius);

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
            _character.StateMachine.ChangeState(_character.StateMachine.States[EnumStateSoul.Idle]);
            return;
        }

        _stateMachine.CurrentObjectInteract = SortObjects(_character.transform.position, _colliderList);

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

        _stateMachine.ChangeState(_stateMachine.States[EnumInteract.Move]);

    }
}

