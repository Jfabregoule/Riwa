using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room3LianaTutorialCollider : MonoBehaviour
{
    private bool _isPlayerInArea;
    private bool _hasBeenAlreadyTriggered = false;
    protected Floor1Room3LevelManager _instance;

    private void Start()
    {
        GameManager.Instance.OnTimeChangeStarted += DialogueToCall;
        DialogueSystem.Instance.OnDialogueEvent += EventDispatcher;
        _instance = (Floor1Room3LevelManager)Floor1Room3LevelManager.Instance;
    }

    public void DialogueToCall(EnumTemporality temporality)
    {
        if (_isPlayerInArea && _hasBeenAlreadyTriggered == false && temporality == EnumTemporality.Present)
        {
            _instance.ChawaTrail.gameObject.SetActive(false);
            _instance.RiwaShowingPathTriggerZone.CanInteract = false;
            _instance.TreeStumpTest.CanInteract = false;

            _hasBeenAlreadyTriggered = true;
            StartCoroutine(_instance.TutorialRoom3Manager.MoveAndOrientChawaToLiana());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ACharacter chara))
            _isPlayerInArea = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out ACharacter chara))
            _isPlayerInArea = false;
    }

    public void EventDispatcher(DialogueEventType eventType)
    {
        switch (eventType)
        {
            case DialogueEventType.EnableStumpRoom3:
                _instance.TreeStumpTest.CanInteract = true;
                break;
        }
    }

}