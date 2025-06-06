using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] MonoBehaviour[] _activableComponents;
    private int _currentActivated = 0;

    [SerializeField] private bool _canBeEntered = true;
    [SerializeField] private int _floorNumber = 1;
    [SerializeField] private int _roomNumber = 1;
    [SerializeField] private DoorDirection _nextDoorDirection;
    [SerializeField] private int _nextDoorID = 0;

    [SerializeField] bool _animationExit = false;

    [SerializeField] Sequencer _enterDoorSequencer;
    [SerializeField] Sequencer _exitDoorSequencer;

    private bool _isExitingDoor = false;
    private bool _isEnteringDoor = false;

    private BoxCollider _collider;

    private void Start()
    {
        _collider = GetComponent<BoxCollider>();
        if (_enterDoorSequencer)
            _enterDoorSequencer.Init();
        if(_exitDoorSequencer)
            _exitDoorSequencer.Init();

        foreach (var activable in _activableComponents)
        {
            if (activable.TryGetComponent(out IActivable act))
            {
                act.OnActivated += OnActivableActivated;
                act.OnDesactivated += OnActivableDeactivated;
            }
        }

        if (_activableComponents.Length > 0)
        {
            DisableDoor();
        }
        else
        {
            EnableDoor();
        }
    }

    private void OnDisable()
    {
        foreach (IActivable activable in _activableComponents)
        {
            activable.OnActivated -= OnActivableActivated;
            activable.OnDesactivated -= OnActivableDeactivated;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (_canBeEntered == false) return;

        if (other.CompareTag("Player") && !_isExitingDoor && !_isEnteringDoor)
        {
            _isEnteringDoor = true;
            _enterDoorSequencer.InitializeSequence();
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
    public void EnableDoor()
    {
        _collider.isTrigger = true;
    }
    public void DisableDoor()
    {
        _collider.isTrigger = false;
    }

    private void OnActivableActivated()
    {
        _currentActivated += 1;
        CheckDoorState();
    }

    private void OnActivableDeactivated()
    {
        _currentActivated -= 1;
    }

    private void CheckDoorState()
    {
        if (_currentActivated == _activableComponents.Length)
        {
            EnableDoor();
            foreach (IActivable activable in _activableComponents)
            {
                activable.OnActivated -= OnActivableActivated;
                activable.OnDesactivated -= OnActivableDeactivated;
            }
        }
    }
}
