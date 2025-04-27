using UnityEngine;

public class RiwaShowingPathTriggerZone : MonoBehaviour
{

    protected bool _isPlayerInArea = false;
    protected Floor1Room3LevelManager _instance;

    private void Start()
    {
        _instance = (Floor1Room3LevelManager)Floor1Room3LevelManager.Instance;
        GameManager.Instance.OnTimeChangeStarted += DialogueToCall;
    }

    public void DialogueToCall(EnumTemporality temporality)
    {
        if (_isPlayerInArea && temporality == EnumTemporality.Present)
        {
            Debug.Log("RiwaShowPathTriggerZone");
            DialogueSystem.Instance.BeginDialogue(_instance.TutorialRoom3Manager.Room3Dialogue[2]);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ACharacter chara))
        {
            _isPlayerInArea = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out ACharacter chara))
            _isPlayerInArea = false;
    }
}