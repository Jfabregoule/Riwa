using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Move Forward Sequence", menuName = "Riwa/GenericAction/MoveForward")]
public class SequencerActionMoveForward : SequencerAction
{
    [SerializeField] private bool _playerBeginInHisOwnPos = false;
    [SerializeField] private bool _waitForEndOfPath= true;
    [SerializeField] private float _secondesBeforeSkip = 0f;

    [SerializeField] private Vector3 _startPosition = Vector3.zero;
    [SerializeField] private Vector3 _targetPosition = Vector3.zero;

    private bool _isMoving;
    private ACharacter _chara;

    public override void Initialize(GameObject obj)
    {
        _isMoving = false;
        _chara = GameManager.Instance.Character;
    }

    public override IEnumerator StartSequence(Sequencer context)
    {
        if (!_playerBeginInHisOwnPos) 
            _chara.transform.position = _startPosition; 

        _isMoving = true;
        _chara.OnMoveToFinished += FinishMoveto;
        _chara.MoveTo(_targetPosition, _targetPosition);

        if (_waitForEndOfPath)
        {
            while (_isMoving)
            {
                yield return null;
            }
        }
        else
        {
            yield return new WaitForSeconds(_secondesBeforeSkip);
        }

        _chara.OnMoveToFinished -= FinishMoveto;

        yield break;
    }

    public void FinishMoveto() {
        _isMoving = false;
    }

}
