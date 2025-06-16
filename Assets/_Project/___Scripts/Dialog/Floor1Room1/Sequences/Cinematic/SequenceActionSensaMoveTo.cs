using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Sensa MoveTo", menuName = "Riwa/Dialogue/Floor1/Room1/Sequences/Sensa MoveTo")]
public class SequenceActionSensaMoveTo : SequencerAction
{
    public bool MoveToSocle = true;
    public bool MoveOnLiana = false;
    public bool MoveToElevator = false;
    
    private bool _isMoving;
    private Floor1Room1LevelManager _instance;
    private ACharacter _chara;

    public override void Initialize(GameObject obj)
    {
        _instance = (Floor1Room1LevelManager)Floor1Room1LevelManager.Instance;
        _chara = GameManager.Instance.Character;
    }

    public override IEnumerator StartSequence(Sequencer context)
    {
        Debug.Log("Appel de SO : " + name);
        _isMoving = true;
        _chara.OnMoveToFinished += FinishMoveTo;
        _chara.WalkSpeed = 1f;
        Vector3 targetPos = _chara.transform.position;
        
        if(MoveToSocle == true)
            targetPos = _instance.EndGameCinematic.Socle.position;
        
        if(MoveOnLiana == true)
            targetPos = _instance.EndGameCinematic.LianaLandTransform.position;
        
        if (MoveToElevator == true)
        {
            targetPos = _instance.EndGameCinematic.ElevatorTransform.position;
            _chara.WalkSpeed = 0.8f;
        }

        MoveToStateCharacter state = (MoveToStateCharacter)_chara.StateMachine.States[EnumStateCharacter.MoveTo];
        state.LoadState(EnumStateCharacter.Idle, targetPos, targetPos);
        _chara.StateMachine.ChangeState(state);

        while (_isMoving)
            yield return null;

        _chara.OnMoveToFinished -= FinishMoveTo;
    }

    private void FinishMoveTo()
    {
        _isMoving = false;
    }
}
