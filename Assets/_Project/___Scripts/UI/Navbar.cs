using UnityEngine;

public class Navbar : MonoBehaviour
{
    [SerializeField] private CanvasGroup _optionsButtonCanvasGroup;
    [SerializeField] private CanvasGroup _navBarCanvasGroup;

    private InputManager _inputManager;

    void OnEnable()
    {
        StartCoroutine(Helpers.WaitMonoBeheviour(() => InputManager.Instance, SubscribeToInputManager));
    }

    void OnDisable()
    {
        if (_inputManager != null)
        {
            _inputManager.OnOpenOptions -= OpenOptions;
        }
    }
    private void OpenOptions()
    {
        _inputManager.DisableGameplayControls();
        _inputManager.DisableDialogueControls();
        Helpers.DisabledCanvasGroup(_optionsButtonCanvasGroup);
        Helpers.EnabledCanvasGroup(_navBarCanvasGroup);
    }

    public void CloseOptions()
    {
        _inputManager.EnableGameplayControls();
        _inputManager.EnableDialogueControls();
    }
    public void DisableCanvasGroup(CanvasGroup canvasGroup)
    {
        Helpers.DisabledCanvasGroup(canvasGroup);
    }
    public void EnableCanvasGroup(CanvasGroup canvasGroup)
    {
        Helpers.EnabledCanvasGroup(canvasGroup);
    }

    private void SubscribeToInputManager(InputManager script)
    {
        if (script != null)
        {
            _inputManager = script;
            _inputManager.OnOpenOptions += OpenOptions;
        }
    }
}
