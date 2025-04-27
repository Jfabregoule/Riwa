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
        _instance = (Floor1Room3LevelManager)Floor1Room3LevelManager.Instance;
    }

    public void DialogueToCall(EnumTemporality temporality)
    {
        if (_isPlayerInArea && _hasBeenAlreadyTriggered == false && temporality == EnumTemporality.Present)
        {
            _hasBeenAlreadyTriggered = true;
            DialogueSystem.Instance.BeginDialogue(_instance.TutorialRoom3Manager.LianaDialogue);
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
}