using UnityEngine;

public class HUD : MonoBehaviour
{
    private bool _isInteracting = true;
    [SerializeField] private GameObject _videoPlayer;

    private void OnEnable()
    {
        GameManager.Instance.Character.OnInteractStarted += () => _isInteracting = true;
        GameManager.Instance.Character.OnInteractEnded += () => _isInteracting = false;
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

}
