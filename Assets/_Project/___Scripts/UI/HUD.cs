using UnityEngine;

public class HUD : MonoBehaviour
{
    private GameManager _gameManager;
    private bool _isInteracting = true;
    [SerializeField] private GameObject _videoPlayer;

    private void OnEnable()
    {
        StartCoroutine(Helpers.WaitMonoBeheviour(() => GameManager.Instance, SubscribeToGameManager));
    }

    private void OnDisable()
    {
        if(_gameManager != null)
        {
            _gameManager.Character.OnInteractStarted += () => _isInteracting = true;
            _gameManager.Character.OnInteractEnded += () => _isInteracting = false;
        }
    }

    public void ChangeTime()
    {
        GameManager.Instance.Character.TriggerChangeTempo();
    }

    public void ToggleInteract()
    {
        if (_isInteracting)
        {
            GameManager.Instance.Character.InputManager.InteractTrue();
        }
        else
        {
            GameManager.Instance.Character.InputManager.InteractFalse();
        }
        _isInteracting = !_isInteracting;
    }

    private void SubscribeToGameManager(GameManager script)
    {
        if (script != null)
        {
            _gameManager = script;
            _gameManager.Character.OnInteractStarted += () => _isInteracting = true;
            _gameManager.Character.OnInteractEnded += () => _isInteracting = false;
        }
    }

}
