using UnityEngine;

public class RiwaHeartTrigger : MonoBehaviour
{

    [SerializeField] private Sequencer _sequencer;
    private Floor1Room1LevelManager _instance;
    private bool _gameEndTriggered = false;

    private void Start()
    {
        _instance = (Floor1Room1LevelManager)Floor1Room1LevelManager.Instance;
        _sequencer.Init();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && _gameEndTriggered == false)
        {
            _gameEndTriggered = true;
            GameManager.Instance.Character.InputManager.DisableGameplayControls();
            _sequencer.InitializeSequence();
        }
    }

}
