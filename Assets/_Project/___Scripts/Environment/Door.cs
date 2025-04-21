using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private int _floorNumber = 1;
    [SerializeField] private int _roomNumber = 1;
    [SerializeField] private DoorDirection _nextDoorDirection;
    [SerializeField] private int _nextDoorID = 0;

    [SerializeField] bool _animationExit = false;
    Sequencer _sequencer;

    public void Start()
    {
        _sequencer = GetComponent<Sequencer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        _sequencer.Init();

        if (other.CompareTag("Player"))
        {
            if (_animationExit)
            {
                _sequencer.InitializeSequence();
            }
            else
            {
                RiwaLoadSceneSystem.Instance.GoToNewScene(_floorNumber, _roomNumber, _nextDoorID, _nextDoorDirection);
            }
        }
    }
}
