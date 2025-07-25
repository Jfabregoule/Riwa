using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;

[Serializable]
public enum DoorDirection
{
    Null,
    North,
    South,
    East,
    West
}

public class RiwaLoadSceneSystem : LoadSceneSystem<RiwaLoadSceneSystem>
{
    private int _currentFloorNum = 0;
    private int _currentRoomNum = 0;

    private int _nextDoorID;
    private DoorDirection _nextDoorDirection = DoorDirection.North;

    private void Start()
    {
        EnqueueScenes(new[] { new SceneData("MainMenu"), new SceneData("HUD", 0, new Action[] { RiwaCinematicSystem.Instance.SetCanvasGroup, SaveSystem.Instance.LoadSettingsData }) }, false);
    }
        
    public void LoadFirstScene()
    {
        StartCoroutine(LoadFirstSceneCoroutine());
    }

    public IEnumerator LoadFirstSceneCoroutine()
    {
        LoadSceneData();
        GameManager.Instance.UIManager.BlackScreen.SetAlpha(1);
        yield return StartCoroutine(ChangeScene(new[] { new SceneData("MainMenu")}, new[] { new SceneData(GetCurrentRoomSceneName())}));
        SpawnPlayerToDoor();
    }

    private void LoadSceneData()
    {
        _currentFloorNum = SaveSystem.Instance.LoadElement<int>("CurrentFloor");
        _currentRoomNum = SaveSystem.Instance.LoadElement<int>("CurrentRoom");
        _nextDoorID = SaveSystem.Instance.LoadElement<int>("LastDoorID");
        _nextDoorDirection = SaveSystem.Instance.LoadElement<DoorDirection>("LastDoorDirection");
    }

    public void GoToNewScene(int floor, int room, int nextDoorID, DoorDirection nextDoorDirection)
    {
        SaveSystem.Instance.SaveElement<int>("CurrentFloor", floor);
        SaveSystem.Instance.SaveElement<int>("CurrentRoom", room);
        SaveSystem.Instance.SaveElement<int>("LastDoorID", nextDoorID);
        SaveSystem.Instance.SaveElement<DoorDirection>("LastDoorDirection", nextDoorDirection);
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

        if (!string.IsNullOrEmpty(currentRoomName))
        {
            GameManager.Instance.UIManager.BlackScreen.SetAlpha(1);
            yield return StartCoroutine(ChangeScene(new[] { new SceneData(currentRoomName) }, new[] { new SceneData(newRoomName) }));
            SpawnPlayerToDoor();
        }
    }

    public void ReloadCurrentScene()
    {
        if (SceneManager.GetActiveScene().name == "Systems")
        {
            Debug.LogWarning("Tentative de reload hors d'une scène gameplay - Ignoré.");
            return;
        }

        Physics.IgnoreLayerCollision(GameManager.Instance.Character.gameObject.layer, Mathf.Clamp(Mathf.RoundToInt(Mathf.Log(GameManager.Instance.Character.PastLayer.value, 2)), 0, 31), true);
        Physics.IgnoreLayerCollision(GameManager.Instance.Character.gameObject.layer, Mathf.Clamp(Mathf.RoundToInt(Mathf.Log(GameManager.Instance.Character.PresentLayer.value, 2)), 0, 31), false);
        GameManager.Instance.CurrentTemporality = EnumTemporality.Present;
        StartCoroutine(ReloadCurrentSceneCoroutine());
    }

    /// <summary>
    /// Recharge la scene actuelle.
    /// Les autres scènes nécessaires ne seront pas affectées.
    /// </summary>
    private IEnumerator ReloadCurrentSceneCoroutine()
    {
        string currentRoomName = GetCurrentRoomSceneName();

        DialogueSystem.Instance.FinishDialogue();

        yield return StartCoroutine(ChangeScene(new[] { new SceneData(currentRoomName) }, new[] {  new SceneData(currentRoomName) }));

        SpawnPlayerToDoor();
    }

    /// <summary>
    /// Récupère le nom de la scène de la salle actuellement chargée.
    /// </summary>
    /// <returns>Le nom de la scène de la salle actuellement chargée.</returns>
    public string GetCurrentRoomSceneName()
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

        if (!targetDoor) return;

        if (targetDoor.TryGetComponent(out Door door))
        {
            GameObject player = GameManager.Instance.Character.gameObject;

            if (player != null)
            {
                player.transform.position = targetDoor.transform.position;
                player.transform.rotation = targetDoor.transform.rotation;
            }
  
            door?.ExitDoor();
        }
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
