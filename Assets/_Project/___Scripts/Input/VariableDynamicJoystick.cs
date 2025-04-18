using UnityEngine;

public class VariableDynamicJoystick : MonoBehaviour
{
    private InputManager _inputManager;
    private CanvasGroup _canvasGroup;
    private RectTransform _background;
    private RectTransform _handle;        

    void OnEnable()
    {
        StartCoroutine(Helpers.WaitMonoBeheviour(() => InputManager.Instance, SubscribeToInputManager));

        _canvasGroup = GetComponentInChildren<CanvasGroup>();
        RectTransform[] rects = _canvasGroup.GetComponentsInChildren<RectTransform>();
        _background = rects[0];
        _handle = rects[1];
    }

    void OnDisable()
    {
        if(_inputManager != null)
        {
            _inputManager.OnInteract -= OnTouchStarted;
            _inputManager.OnInteractEnd -= OnTouchEnded;
        }
    }

    private void OnTouchStarted()
    {
        Helpers.EnabledCanvasGroup(_canvasGroup);
        _background.position = _inputManager.GetPressPosition();
        _handle.anchoredPosition = Vector2.zero;
    }

    private void OnTouchEnded()
    {
        Helpers.DisabledCanvasGroup(_canvasGroup);
        _handle.anchoredPosition = Vector2.zero;
    }

    private void SubscribeToInputManager(InputManager script)
    {
        if (script != null)
        {
            _inputManager = script;
            script.OnInteract += OnTouchStarted;
            script.OnInteractEnd += OnTouchEnded;
            Debug.Log("Script is ready!");
        }
        else
        {
            Debug.LogWarning("Script was still null after timeout.");
        }
    }
}
