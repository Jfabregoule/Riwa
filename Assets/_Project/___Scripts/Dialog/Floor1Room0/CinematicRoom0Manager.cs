using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicRoom0Manager : MonoBehaviour
{
    [SerializeField] private DialogueAsset _dialogueAsset;
    [SerializeField] private Sequencer _sequencerEntry;

    private Floor1Room0LevelManager _instance;
    private bool _isTrigger;

    private System.Action OnFinishSpeak;

    public DialogueAsset Room0Dialogue { get => _dialogueAsset; }
    private void OnEnable()
    {
        LoadData();
        SaveSystem.Instance.OnLoadProgress += LoadData;
    }

    private void OnDisable()
    {
        SaveSystem.Instance.OnLoadProgress -= LoadData;
        SaveSystem.Instance.SaveElement<bool>("DialogRoom0", _isTrigger);
    }

    private void LoadData()
    {
        _isTrigger = SaveSystem.Instance.LoadElement<bool>("DialogRoom0");
    }

    private void Start()
    {
        _instance = (Floor1Room0LevelManager)Floor1Room0LevelManager.Instance;
        _instance.OnLevelEnter += Init;
        //_sequencerEntry.Init();
        //DialogueSystem.Instance.OnDialogueEvent += DispatchEventOnDialogueEvent;
        //DialogueSystem.Instance.BeginDialogue(_dialogueAsset);
    }

    private void Init()
    {
        if (!_isTrigger)
        {
            _sequencerEntry.Init();
            DialogueSystem.Instance.OnDialogueEvent += DispatchEventOnDialogueEvent;
            _sequencerEntry.InitializeSequence();
            _isTrigger = true;
        }
    }

    private void DispatchEventOnDialogueEvent(DialogueEventType dialogueEvent)
    {
        switch(dialogueEvent)
        {
            case DialogueEventType.AntreRiwaChangeTempo:
                GameManager.Instance.Character.TriggerChangeTempoWithouCooldown();
                break;
            case DialogueEventType.AntreRiwaCinematicEnd:
                _instance.IsCinematicDone = true;
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
