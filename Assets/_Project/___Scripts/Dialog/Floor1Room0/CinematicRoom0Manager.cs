using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicRoom0Manager : MonoBehaviour
{
    [SerializeField] private DialogueAsset _dialogueAsset;
    [SerializeField] private DialogueAsset _collectibleAsset;
    [SerializeField] private DialogueAsset _closeAsset;
    [SerializeField] private Sequencer _sequencerEntry;
    [SerializeField] private Sequencer _collectibleSequencer;
    [SerializeField] private Sequencer _closeSequencer;
    [SerializeField] private Transform _collectibleLandingPosition;

    private Floor1Room0LevelManager _instance;
    private bool _isTrigger;

    public delegate void DialogueEnd();

    public event DialogueEnd OnDialogueEnd;

    private System.Action OnFinishSpeak;

    public DialogueAsset Room0Dialogue { get => _dialogueAsset; }
    public DialogueAsset Room0CollectibleDialogue { get => _collectibleAsset; }
    public DialogueAsset Room0CloseDialogue { get => _closeAsset; }
    public Transform CollectibleLandingPosition { get => _collectibleLandingPosition; }

    private void Start()
    {
        _instance = (Floor1Room0LevelManager)Floor1Room0LevelManager.Instance;
        _instance.OnLevelEnter += Init;
    }

    private void Init()
    {
        if (_instance.IsCinematicDone == false)
        {
            _sequencerEntry.Init();
            _collectibleSequencer.Init();
            _closeSequencer.Init();
            
            DialogueSystem.Instance.OnDialogueEvent += DispatchEventOnDialogueEvent;
            
            _sequencerEntry.InitializeSequence();
        }
    }

    private void DispatchEventOnDialogueEvent(DialogueEventType dialogueEvent)
    {
        switch(dialogueEvent)
        {
            case DialogueEventType.Room0CollectibleTaken:
                OnDialogueEnd?.Invoke();
                break;
            case DialogueEventType.Room0CollectibleDialogueEnd:
                _closeSequencer.InitializeSequence();
                break;
            case DialogueEventType.Room0CollectibleInformationEnd:
                _instance.CollectibleCamera.Priority = 0;
                DialogueSystem.Instance.BeginDialogue(_closeAsset);
                break;
            case DialogueEventType.Room0CollectibleCamera:
                _instance.CollectibleCamera.Priority = 200;
                break;
            case DialogueEventType.Room0MoveToCollectible:
                _collectibleSequencer.InitializeSequence();
                break;
            case DialogueEventType.AntreRiwaChangeTempo:
                GameManager.Instance.Character.TriggerChangeTempoWithouCooldown();
                break;
            case DialogueEventType.AntreRiwaCinematicEnd:
                _instance.RiwaSensaCamera.Priority = 0;
                break;
            case DialogueEventType.SensaSpeaking:
                ACharacter chara = (ACharacter)GameManager.Instance.Character;
                chara.OnFinishAnimationSpeak += SkipSpeaking;
                chara.LaunchSensaSpeakingAnimation();
                DialogueSystem.Instance.EventRegistery.Register(WaitDialogueEventType.SensaFinishToSpeak, OnFinishSpeak);
                break;
        }
    }

    private void SkipSpeaking()
    {
        DialogueSystem.Instance.EventRegistery.Invoke(WaitDialogueEventType.SensaFinishToSpeak);

        ACharacter chara = (ACharacter)GameManager.Instance.Character;
        chara.OnFinishAnimationSpeak -= SkipSpeaking;
    }

}
