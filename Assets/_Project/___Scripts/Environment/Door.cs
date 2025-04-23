using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private int _floorNumber = 1;
    [SerializeField] private int _roomNumber = 1;
    [SerializeField] private DoorDirection _nextDoorDirection;
    [SerializeField] private int _nextDoorID = 0;

    [SerializeField] bool _animationExit = false;

    [SerializeField] Sequencer _enterDoorSequencer;
    [SerializeField] Sequencer _exitDoorSequencer;

    private bool _isExitingDoor = false;
    private bool _isEnteringDoor = false;

    private void Start()
    {
        if (_enterDoorSequencer)
            _enterDoorSequencer.Init();
        if(_exitDoorSequencer)
            _exitDoorSequencer.Init();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_isExitingDoor && !_isEnteringDoor)
        {
            if (_animationExit)
            {
                _isEnteringDoor = true;
                _enterDoorSequencer.InitializeSequence();
            }
        }
    }

    public void ExitDoor()
    {
        _isExitingDoor = true;
        _exitDoorSequencer.InitializeSequence();
    }

    public void ResetDoor()
    {
        _isExitingDoor = false;
        _isEnteringDoor = false;
        GameManager.Instance.CurrentLevelManager.LevelEnter();
    }

    public void ChangeScene()
    {
        RiwaLoadSceneSystem.Instance.GoToNewScene(_floorNumber, _roomNumber, _nextDoorID, _nextDoorDirection);
    }
}
