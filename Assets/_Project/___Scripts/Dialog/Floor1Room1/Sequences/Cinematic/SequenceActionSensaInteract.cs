using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sensa Interact", menuName = "Riwa/Dialogue/Floor1/Room1/Sequences/Sensa Or Soul Interact")]
public class SequenceActionSensaInteract : SequencerAction
{

    private ACharacter _character;
    private ASoul _soul;
    
    public bool SensaInteract = true;
    public bool SoulInteract = false;
    
    public override void Initialize(GameObject obj)
    {
        _character = GameManager.Instance.Character;
        _soul = GameManager.Instance.Character.Soul.GetComponent<ASoul>();
    }

    public override IEnumerator StartSequence(Sequencer context)
    {
        if (SensaInteract)
        {
            InteractStateCharacter state = (InteractStateCharacter)_character.StateMachine.States[EnumStateCharacter.Interact];
            _character.StateMachine.ChangeState(state);
            _character.InteractBegin();
            yield return null;
        }

        if (SoulInteract)
        {
            InteractStateSoul soulInteractState = (InteractStateSoul)_soul.StateMachine.States[EnumStateSoul.Interact];
            _soul.StateMachine.ChangeState(soulInteractState);
            _soul.InteractBegin();
            yield return null;
        }
    }
}
