using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room3DialogTriggerZoneParent : MonoBehaviour
{
    protected bool _isPlayerInArea = false;
    protected Floor1Room3LevelManager _instance;

    private void OnEnable()
    {
        GameManager.Instance.OnTimeChangeStarted += DialogueToCall;
    }

    private void OnDisable()
    {
        if (GameManager.Instance == null) return;
        GameManager.Instance.OnTimeChangeStarted -= DialogueToCall;
    }

    private void Start()
    {
        _instance = (Floor1Room3LevelManager)Floor1Room3LevelManager.Instance;
    }

    public virtual void DialogueToCall(EnumTemporality temporality) { }

    private void OnTriggerEnter(Collider other)
    {
        _isPlayerInArea = true;
    }

    private void OnTriggerExit(Collider other)
    {
        _isPlayerInArea = false;
    }
}
