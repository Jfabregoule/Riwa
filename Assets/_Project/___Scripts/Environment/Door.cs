using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private int _floorNumber = 1;
    [SerializeField] private int _roomNumber = 1;
    [SerializeField] private DoorDirection _nextDoorDirection;
    [SerializeField] private int _nextDoorID = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            RiwaLoadSceneSystem.Instance.GoToNewScene(_floorNumber, _roomNumber, _nextDoorID, _nextDoorDirection);
        }
    }
}
