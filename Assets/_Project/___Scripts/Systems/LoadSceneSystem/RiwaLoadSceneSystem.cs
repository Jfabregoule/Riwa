using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public enum DoorDirection
{
    North,
    South,
    East,
    West
}

public class RiwaLoadSceneSystem : LoadSceneSystem<RiwaLoadSceneSystem>
{
    private int _currentFloorNum = 1;
    private int _currentRoomNum = 1;

    [SerializeField] private float _spawnOffset;
    private int _nextDoorID;
    private DoorDirection _nextDoorDirection;

    //private void Start()
    //{
    //    EnqueueScenes(new[] { new SceneData("HUD"), new SceneData("Floor1Room1") }, false);
    //}

    public void GoToNewScene(int floor, int room, int nextDoorID, DoorDirection nextDoorDirection)
    {
        SetNextSpawnInfo(nextDoorID, nextDoorDirection);
        StartCoroutine(LoadRoomScene(floor, room));
    }

    /// <summary>
    /// Charge une nouvelle scène de salle tout en déchargeant la salle actuelle.
    /// Les autres scènes nécessaires ne seront pas affectées.
    /// </summary>
    /// <param name="floor">Numéro de l'étage.</param>
    /// <param name="room">Numéro de la salle.</param>
    public IEnumerator LoadRoomScene(int floor, int room)
    {
        string currentRoomName = GetCurrentRoomSceneName();
        string newRoomName = $"Floor{floor}Room{room}";

        _currentFloorNum = floor;
        _currentRoomNum = room;

        if (!string.IsNullOrEmpty(currentRoomName) && currentRoomName != newRoomName)
        {
            yield return StartCoroutine(ChangeScene(new[] { new SceneData(currentRoomName) }, new[] { new SceneData(newRoomName) }));
            SpawnPlayerToDoor();
        }
    }

    /// <summary>
    /// Récupère le nom de la scène de la salle actuellement chargée.
    /// </summary>
    /// <returns>Le nom de la scène de la salle actuellement chargée.</returns>
    private string GetCurrentRoomSceneName()
    {
        return $"Floor{_currentFloorNum}Room{_currentRoomNum}";
    }

    public void SetNextSpawnInfo(int targetDoorID, DoorDirection entryDirection)
    {
        _nextDoorID = targetDoorID;
        _nextDoorDirection = entryDirection;
    }

    private void SpawnPlayerToDoor()
    {
        GameObject targetDoor = GameObject.Find(_nextDoorID == 0 ? $"Door{_nextDoorDirection}" : $"Door{_nextDoorDirection}{_nextDoorID}");

        if (targetDoor == null) return;

        Quaternion rotation = GetRotationFromDirection(_nextDoorDirection);

        GameObject.FindGameObjectWithTag("Player").transform.SetPositionAndRotation(targetDoor.transform.position + targetDoor.transform.forward * _spawnOffset, rotation);
    }

    private Quaternion GetRotationFromDirection(DoorDirection direction)
    {
        return direction switch
        {
            DoorDirection.North => Quaternion.LookRotation(Vector3.back),
            DoorDirection.South => Quaternion.LookRotation(Vector3.forward),
            DoorDirection.East => Quaternion.LookRotation(Vector3.left),
            DoorDirection.West => Quaternion.LookRotation(Vector3.right),
            _ => Quaternion.identity
        };
    }
}
