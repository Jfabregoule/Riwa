using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractStateCharacter : BaseStateCharacter
{

    private readonly List<GameObject> _colliderList = new();
    private float _currentOffset;

    public override void InitState(StateMachineCharacter stateMachine, EnumStateCharacter enumValue, ACharacter character)
    {
        base.InitState(stateMachine, enumValue, character);
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
        point1 += _character.Pawn.transform.forward * radius * 1.5f;
        point2 += _character.Pawn.transform.forward * radius * 1.5f;

        Collider[] others = Physics.OverlapCapsule(point1, point2, radius, layerMask);

        foreach (Collider collider in others)
        {
            if(collider.gameObject.TryGetComponent<IInteractable>(out IInteractable obj))
            {
                _colliderList.Add(collider.gameObject);
            }
        }

        if(_colliderList.Count <= 0)
        {
            //Si il n'y a aucun obj interactaible, rien ne se passes
            _stateMachine.ChangeState(_stateMachine.States[EnumStateCharacter.Idle]);
            return;
        }

        GameObject objInteractable = SortObjects();

        objInteractable.GetComponent<IInteractable>().Interactable();
        _currentOffset = objInteractable.GetComponent<IInteractable>().OffsetRadius;

        if (objInteractable.TryGetComponent<IHoldable>(out IHoldable holdable))
        {
            _stateMachine.ChangeState(_stateMachine.States[EnumStateCharacter.Holding]);
            return;
        }

        if (objInteractable.TryGetComponent<ITreeStump>(out ITreeStump stump))
        {
            _stateMachine.ChangeState(_stateMachine.States[EnumStateCharacter.Soul]);
            return;
        }

        //Default
        _stateMachine.ChangeState(_stateMachine.States[EnumStateCharacter.Idle]);


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

    public GameObject SortObjects()
    {
        ///<summary>
        /// Renvoie l'object interactable le plus proche du player
        /// </summary>

        GameObject closestObj = _colliderList[0];
        Vector3 playerPos = Character.Pawn.transform.position;

        float distance = Vector3.Distance(closestObj.transform.position, playerPos);

        for(int i = 1; i < _colliderList.Count; i++)
        {
            if (Vector3.Distance(_colliderList[i].transform.position, playerPos) < distance)
            {
                closestObj = _colliderList[i];
                distance = Vector3.Distance(closestObj.transform.position, playerPos);
            }
        }

        return closestObj;

    }
         
}
