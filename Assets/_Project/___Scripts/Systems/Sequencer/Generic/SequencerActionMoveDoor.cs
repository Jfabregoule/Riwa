using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Move Door Sequence", menuName = "Riwa/GenericAction/MoveDoor")]
public class SequencerActionMoveDoor : SequencerAction
{
    [SerializeField] private bool _isEntering = false;
    [SerializeField] private float _distanceToDoor = 2f;

    private GameObject _door;

    private bool _isMoving;
    private ACharacter _chara;

    public override void Initialize(GameObject obj)
    {
        _isMoving = false;
        _chara = GameManager.Instance.Character;
        _door = obj;
    }

    public override IEnumerator StartSequence(Sequencer context)
    {
        _door = context.gameObject;
        GameManager.Instance.InvokeRoomExit();

        _isMoving = true;
        _chara.OnMoveToFinished += FinishMoveto;

        Vector3 target;

        if (_isEntering)
        {
            target = _door.transform.position - _door.transform.forward * _distanceToDoor;
        }
        else
        { 
            target = _door.transform.position + _door.transform.forward * _distanceToDoor;
        }

        MoveToStateCharacter state = (MoveToStateCharacter)_chara.StateMachine.States[EnumStateCharacter.MoveTo];
        state.LoadState(EnumStateCharacter.Idle,target, target);
        _chara.StateMachine.ChangeState(state);
        //_chara.MoveTo(target, target);

        while (_isMoving)
        {
            yield return null;
        }

        _chara.OnMoveToFinished -= FinishMoveto;

        yield break;
    }

    public void FinishMoveto() 
    {
        _isMoving = false;
    }

}
