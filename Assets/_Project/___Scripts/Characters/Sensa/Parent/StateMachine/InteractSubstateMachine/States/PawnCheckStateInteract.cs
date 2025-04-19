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

        float security = 3f;

        float heigth = _character.CapsuleCollider.height * _character.transform.localScale.x;

        Vector3 point1 = _character.transform.position;
        Vector3 point2 = _character.transform.position + Vector3.up * heigth;
        float radius = _character.CapsuleCollider.radius * _character.transform.localScale.x * security;

        //Offset pour mettre la capsule devant le joueur
        point1 += _character.transform.forward * radius * 0.8f;
        point2 += _character.transform.forward * radius * 0.8f;

        LayerMask layerMask = _character.IsInPast ? _character.PastLayer : _character.PresentLayer;

        Collider[] others = Physics.OverlapCapsule(point1, point2, radius, layerMask);
        Debug.DrawLine(point2, point2 + Vector3.right * radius, Color.white, 10f);
        Debug.DrawLine(point1, point1 + Vector3.right * radius, Color.magenta, 10f);

        Debug.DrawLine(point2, point2 + Vector3.forward * radius, Color.white, 10f);
        Debug.DrawLine(point1, point1 + Vector3.forward * radius, Color.magenta, 10f);

        Debug.DrawLine(point2, point2 + -Vector3.right * radius, Color.white, 10f);
        Debug.DrawLine(point1, point1 + -Vector3.right * radius, Color.magenta, 10f);

        Debug.DrawLine(point2, point2 + -Vector3.forward * radius, Color.white, 10f);
        Debug.DrawLine(point1, point1 + -Vector3.forward * radius, Color.magenta, 10f);

        foreach (Collider collider in others)
        {
            if (collider.gameObject.TryGetComponent<IInteractable>(out IInteractable obj))
            {
                RaycastHit hit;
                
                if (Physics.Raycast(_character.transform.position, (collider.gameObject.transform.position - _character.transform.position).normalized, out hit, 5f * _character.transform.localScale.x, layerMask))
                {
                    if (hit.collider.gameObject == collider.gameObject)
                    {
                        _colliderList.Add(collider.gameObject);
                    }
                }
            }
        }

        if (_colliderList.Count <= 0)
        {
            //Si il n'y a aucun obj interactaible, rien ne se passes
            ChangeStateToIdle();
            return;
        }

        _subStateMachine.CurrentObjectInteract = SortObjects(_character.transform.position, _colliderList);

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
        ChangeStateToMove();
    }

    public virtual void ChangeStateToIdle() { }
    public virtual void ChangeStateToMove() { }

}
