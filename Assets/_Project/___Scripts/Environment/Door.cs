using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private int _floorNumber = 1;
    [SerializeField] private int _roomNumber = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            RiwaLoadSceneSystem.Instance.LoadRoomScene(_floorNumber, _roomNumber);
        }
    }
}
