using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sensa toward Riwa", menuName = "Riwa/Dialogue/Floor1/Room0/Sequences/Sensa toward Riwa")]
public class SequenceActionRoom0SensaTowardRiwa : SequencerAction
{

    public bool BeginDialogue = true;
    public bool AccelerateMovement = true;
    private Floor1Room0LevelManager _instance;
    private bool _isMoving;
    private ACharacter _chara;

    public override void Initialize(GameObject obj)
    {
        _instance = (Floor1Room0LevelManager)Floor1Room0LevelManager.Instance;
        _isMoving = false;
        _chara = GameManager.Instance.Character;
    }

    public override IEnumerator StartSequence(Sequencer context)
    {
        _isMoving = true;
        _chara.OnMoveToFinished += FinishMoveto;

        Vector3 landPos = _instance.SensaLandPos.position;
        Vector3 target = landPos;

        _chara.WalkSpeed *= AccelerateMovement ? 5 : 1;
        MoveToStateCharacter state = (MoveToStateCharacter)_chara.StateMachine.States[EnumStateCharacter.MoveTo];
        state.LoadState(EnumStateCharacter.Idle, target, target);
        _chara.StateMachine.ChangeState(state);

        while(_isMoving)
        {
            yield return null;
        }

        _chara.OnMoveToFinished -= FinishMoveto;

        yield break;
    }

    public void FinishMoveto()
    {
        _isMoving = false;
        _chara.WalkSpeed = 2f;
        if(BeginDialogue == true) DialogueSystem.Instance.BeginDialogue(_instance.CinematicManager.Room0Dialogue);
    }
}
