public class RiwaLoadSceneSystem : LoadSceneSystem<RiwaLoadSceneSystem>
{
    private int _currentFloorNum = 1;
    private int _currentRoomNum = 1;

    private void Start()
    {
        EnqueueScenes(new[] { new SceneData("HUD"), new SceneData("Floor1Room1") }, false);
    }

    /// <summary>
    /// Charge une nouvelle scène de salle tout en déchargeant la salle actuelle.
    /// Les autres scènes nécessaires ne seront pas affectées.
    /// </summary>
    /// <param name="floor">Numéro de l'étage.</param>
    /// <param name="room">Numéro de la salle.</param>
    public void LoadRoomScene(int floor, int room)
    {
        string currentRoomName = GetCurrentRoomSceneName();
        string newRoomName = $"Floor{floor}Room{room}";

        _currentFloorNum = floor;
        _currentRoomNum = room;

        if (!string.IsNullOrEmpty(currentRoomName) && currentRoomName != newRoomName)
        {
            ChangeScene(new[] {new SceneData(currentRoomName)}, new[] { new SceneData(newRoomName) });
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
}
