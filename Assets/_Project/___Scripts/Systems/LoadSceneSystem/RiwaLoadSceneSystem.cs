public class RiwaLoadSceneSystem : LoadSceneSystem<RiwaLoadSceneSystem>
{
    private int _currentFloorNum = 1;
    private int _currentRoomNum = 1;

    private void Start()
    {
        EnqueueScenes(new[] { new SceneData("HUD"), new SceneData("Floor1Room1") }, false);
    }

    /// <summary>
    /// Charge une nouvelle sc�ne de salle tout en d�chargeant la salle actuelle.
    /// Les autres sc�nes n�cessaires ne seront pas affect�es.
    /// </summary>
    /// <param name="floor">Num�ro de l'�tage.</param>
    /// <param name="room">Num�ro de la salle.</param>
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
    /// R�cup�re le nom de la sc�ne de la salle actuellement charg�e.
    /// </summary>
    /// <returns>Le nom de la sc�ne de la salle actuellement charg�e.</returns>
    private string GetCurrentRoomSceneName()
    {
        return $"Floor{_currentFloorNum}Room{_currentRoomNum}";
    }
}
