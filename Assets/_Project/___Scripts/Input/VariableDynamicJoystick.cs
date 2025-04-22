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
    }

    void OnDisable()
    {
        if(_inputManager != null)
        {
            _inputManager.OnMove -= OnTouchStarted;
            _inputManager.OnMoveEnd -= OnTouchEnded;
        }
    }

    private void Start()
    {
        _canvasGroup = GetComponentInChildren<CanvasGroup>();
        RectTransform[] rects = _canvasGroup.GetComponentsInChildren<RectTransform>();
        _background = rects[0];
        _handle = rects[1];
    }

    private void OnTouchStarted(Vector2 position)
    {
        Helpers.EnabledCanvasGroup(_canvasGroup);
        _background.position = position;
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
            script.OnMove += OnTouchStarted;
            script.OnMoveEnd += OnTouchEnded;
        }
    }
}
