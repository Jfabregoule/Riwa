using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Soul MoveTo", menuName = "Riwa/Dialogue/Floor1/Room1/Sequences/Soul MoveTo")]
public class SequenceActionSoulMoveTo : SequencerAction
{

    public bool MoveToBourgeon = true;
    public bool MoveToSensa = false;
    private bool _isMoving;
    private Floor1Room1LevelManager _instance;
    private ASoul _soul;
    
    public override void Initialize(GameObject obj)
    {
        _instance = (Floor1Room1LevelManager)Floor1Room1LevelManager.Instance;
        _soul = GameManager.Instance.Character.Soul.GetComponent<ASoul>();
    }

    public override IEnumerator StartSequence(Sequencer context)
    {
        _soul.OnMoveToFinished += FinishMoveTo;
        _isMoving = true;
        _soul.WalkSpeed *= 0.5f;
        Vector3 landPos = _soul.transform.position;
        if (MoveToBourgeon == true)
        {
            landPos = _instance.EndGameCinematic.Bourgeon.position;
            landPos.x += 0.5f;
            landPos.z += 0.5f;
        }

        if (MoveToSensa == true)
        {
            landPos = GameManager.Instance.Character.transform.position;
            landPos.x -= 0.5f;
            landPos.z -= 0.5f;
        }
        
        landPos.y = _soul.transform.position.y;
        
        MoveStateSoul moveState = (MoveStateSoul)_soul.StateMachine.States[EnumStateSoul.Move];
        _soul.StateMachine.ChangeState(moveState);
        _soul.MoveTo(landPos, landPos);

        while (_isMoving)
            yield return null;

        _soul.OnMoveToFinished -= FinishMoveTo;
    }

    private void FinishMoveTo()
    {
        _isMoving = false;
        IdleStateSoul idleState = (IdleStateSoul)_soul.StateMachine.States[EnumStateSoul.Idle];
        _soul.StateMachine.ChangeState(idleState);
    }
}
